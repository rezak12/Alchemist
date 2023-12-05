using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Code.Infrastructure.Services.AssetProvider
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _handles = new();

        public async UniTask InitializeAsync() =>
            await Addressables.InitializeAsync().ToUniTask();
        
        public async UniTask<T> LoadAsync<T>(string key, bool cacheHandle = true) where T : class
        {
            if (!cacheHandle)
            {
                return await Addressables.LoadAssetAsync<T>(key);
            }
            
            if (!_handles.TryGetValue(key, out AsyncOperationHandle handle))
            {
                handle = Addressables.LoadAssetAsync<T>(key);
                _handles.Add(key, handle);
            }

            await handle.ToUniTask();
            
            return handle.Result as T;
        }

        public async UniTask<T> LoadAsync<T>(AssetReference assetReference, bool cacheHandle = true) where T : class =>
             await LoadAsync<T>(assetReference.AssetGUID, cacheHandle);

        public async UniTask<T[]> LoadAsync<T>(IReadOnlyCollection<string> assetKeys, bool cacheHandle = true) 
            where T : class
        {
            var tasks = new List<UniTask<T>>(assetKeys.Count);
            foreach (var key in assetKeys)
            {
                tasks.Add(LoadAsync<T>(key, cacheHandle));
            }

            return await UniTask.WhenAll(tasks);
        }

        public async UniTask<T[]> LoadAsync<T>(IEnumerable<AssetReference> assetReferences, bool cacheHandle = true) 
            where T : class
        {
            var assetKeys = assetReferences.Select(reference => reference.AssetGUID).ToList();
            return await LoadAsync<T>(assetKeys, cacheHandle);
        }

        public async UniTask WarmupByLabelAsync(string label)
        {
            var assetsList = await GetAssetsListByLabel(label);
            await LoadAsync<object>(assetsList);
        }

        public async UniTask<List<string>> GetAssetsListByLabel<TAsset>(string label) => 
            await GetAssetsListByLabel(label, typeof(TAsset));
        
        private async UniTask<List<string>> GetAssetsListByLabel(string label, Type type = null)
        {
            var operationHandle = Addressables.LoadResourceLocationsAsync(label, type);
            var locations = await operationHandle.ToUniTask();

            List<string> assetKeys = new List<string>(locations.Count);
            foreach (var location in locations)
            {
                assetKeys.Add(location.PrimaryKey);
            }

            Addressables.Release(operationHandle);
            return assetKeys;
        }

        public void Cleanup()
        {
            foreach (var asyncOperationHandle in _handles)
            {
                Addressables.Release(asyncOperationHandle.Value);
            }
            _handles.Clear();
        }
    }
}