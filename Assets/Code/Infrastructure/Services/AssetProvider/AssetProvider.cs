﻿using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Code.Infrastructure.Services.AssetProvider
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();

        public async UniTask InitializeAsync()
        {
            await Addressables.InitializeAsync().ToUniTask();
        }
        
        public async UniTask<T> LoadAsync<T>(AssetReference assetReference) where T : class
        {
            if (_completedCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle cache))
            {
                if(cache.IsValid())
                    return cache.Result as T;
            }
            
            return await RunWithCacheOnComplete(
                Addressables.LoadAssetAsync<T>(assetReference), 
                assetReference.AssetGUID);
        }

        public async UniTask<T> LoadAsync<T>(string address) where T : class
        {
            if (_completedCache.TryGetValue(address, out AsyncOperationHandle cache))
            {
                if(cache.IsValid())
                    return cache.Result as T;
            }

            return await RunWithCacheOnComplete(
                Addressables.LoadAssetAsync<T>(address), 
                address);
        }

        public async UniTask<T[]> LoadAsync<T>(IEnumerable<AssetReference> assetReferences) where T : class
        {
            var tasks = new List<UniTask<T>>();
            foreach (AssetReference reference in assetReferences)
            {
                var task = LoadAsync<T>(reference);
                tasks.Add(task);
            }

            return await UniTask.WhenAll(tasks);
        }

        private async UniTask<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += h => _completedCache[cacheKey] = h;

            AddHandle(cacheKey, handle);

            return await handle.ToUniTask();
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> handles))
            {
                handles = new List<AsyncOperationHandle>();
                _handles[key] = handles;
            }

            handles.Add(handle);
        }

        public void Cleanup()
        {
            foreach (List<AsyncOperationHandle> handles in _handles.Values)
            {
                foreach (AsyncOperationHandle handle in handles)
                {
                    Addressables.Release(handle);
                }
            }
            _completedCache.Clear();
            _handles.Clear();
        }
    }
}