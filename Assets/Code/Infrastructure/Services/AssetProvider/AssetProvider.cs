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
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new();

        public async UniTask InitializeAsync()
        {
            await Addressables.InitializeAsync().ToUniTask();
        }

        public async UniTask<T> LoadAsync<T>(string key) where T : class
        {
            if (!_completedCache.TryGetValue(key, out AsyncOperationHandle handle))
            {
                handle = Addressables.LoadAssetAsync<T>(key);
                _completedCache.Add(key, handle);
            }

            await handle.ToUniTask();
            
            return handle.Result as T;
        }

        public async UniTask<T> LoadAsync<T>(AssetReference assetReference) where T : class
        {
            return await LoadAsync<T>(assetReference.AssetGUID);
        }

        public async UniTask<T[]> LoadAsync<T>(IEnumerable<string> assetKeys) where T : class
        {
            var tasks = new List<UniTask<T>>();
            foreach (var key in assetKeys)
            {
                tasks.Add(LoadAsync<T>(key));
            }

            return await UniTask.WhenAll(tasks);
        }

        public async UniTask<T[]> LoadAsync<T>(IEnumerable<AssetReference> assetReferences) where T : class
        {
            var assetKeys = assetReferences.Select(reference => reference.AssetGUID);
            return await LoadAsync<T>(assetKeys);
        }

        public async UniTask<List<string>> GetAssetsListByLabel<TAsset>(string label) => 
            await GetAssetsListByLabel(label, typeof(TAsset));

        public async UniTask<List<string>> GetAssetsListByLabel(string label, Type type = null)
        {
            var operationHandle = Addressables.LoadResourceLocationsAsync(label, type);

            var locations = await operationHandle.ToUniTask();

            List<string> assetKeys = new List<string>(locations.Count);

            foreach (var location in locations) 
                assetKeys.Add(location.PrimaryKey);
            
            Addressables.Release(operationHandle);
            return assetKeys;
        }

        public async UniTask WarmupAssetsByLabel(string label)
        {
            var assetsList = await GetAssetsListByLabel(label);
            await LoadAsync<object>(assetsList);
        }

        public async UniTask ReleaseAssetsByLabel(string label)
        {
            var assetsList = await GetAssetsListByLabel(label);
            
            foreach (var assetKey in assetsList)
                if (_completedCache.TryGetValue(assetKey, out var handler))
                {
                    Addressables.Release(handler);
                    _completedCache.Remove(assetKey);
                }
        }

        public void Cleanup()
        {
            foreach (var asyncOperationHandle in _completedCache)
            {
                Addressables.Release(asyncOperationHandle.Value);
            }
            _completedCache.Clear();
        }
    }
}