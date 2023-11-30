using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Code.Infrastructure.Services.AssetProvider
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _handles = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _loadedByLabelHandles = new();

        public async UniTask InitializeAsync() =>
            await Addressables.InitializeAsync().ToUniTask();
        

        public async UniTask<T> LoadAsync<T>(string key) where T : class
        {
            if (!_handles.TryGetValue(key, out AsyncOperationHandle handle))
            {
                handle = Addressables.LoadAssetAsync<T>(key);
                _handles.Add(key, handle);
            }

            await handle.ToUniTask();
            
            return handle.Result as T;
        }

        public async UniTask<T> LoadAsync<T>(AssetReference assetReference) where T : class =>
             await LoadAsync<T>(assetReference.AssetGUID);

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

        public async UniTask<GameObject> InstantiateAsync(string key) =>
             await Addressables.InstantiateAsync(key).ToUniTask();
        
        public async UniTask<GameObject> InstantiateAsync(AssetReference reference) =>
             await Addressables.InstantiateAsync(reference).ToUniTask();

        public void Cleanup()
        {
            foreach (var asyncOperationHandle in _handles)
            {
                Addressables.Release(asyncOperationHandle.Value);
            }
            _handles.Clear();
        }

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
    }
}