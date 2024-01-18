using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Code.Infrastructure.Services.AssetProvider
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _handles = new();

        public async UniTask InitializeAsync() =>
            await Addressables.InitializeAsync().ToUniTask();
        
        public async UniTask<TAsset> LoadAsync<TAsset>(string key, bool cacheHandle = true) where TAsset : class
        {
            if (!cacheHandle)
            {
                return await GetLoadingHandle<TAsset>(key);
            }
            
            if (!_handles.TryGetValue(key, out AsyncOperationHandle handle))
            {
                handle = Addressables.LoadAssetAsync<TAsset>(key);
                _handles.Add(key, handle);
            }

            await handle.ToUniTask();
            
            return handle.Result as TAsset;
        }

        public async UniTask<TAsset> LoadAsync<TAsset>(AssetReference assetReference, bool cacheHandle = true) 
            where TAsset : class => await LoadAsync<TAsset>(assetReference.AssetGUID, cacheHandle);

        public async UniTask<TAsset[]> LoadAsync<TAsset>(IReadOnlyCollection<string> assetKeys, bool cacheHandle = true) 
            where TAsset : class
        {
            var tasks = new List<UniTask<TAsset>>(assetKeys.Count);
            foreach (var key in assetKeys)
            {
                tasks.Add(LoadAsync<TAsset>(key, cacheHandle));
            }

            return await UniTask.WhenAll(tasks);
        }

        public async UniTask<TAsset[]> LoadAsync<TAsset>(IEnumerable<AssetReference> assetReferences, bool cacheHandle = true) 
            where TAsset : class
        {
            List<string> assetKeys = assetReferences.Select(reference => reference.AssetGUID).ToList();
            return await LoadAsync<TAsset>(assetKeys, cacheHandle);
        }

        public AsyncOperationHandle<TAsset> GetLoadingHandle<TAsset>(string key) where TAsset : class => 
            Addressables.LoadAssetAsync<TAsset>(key);

        public AsyncOperationHandle<TAsset> GetLoadingHandle<TAsset>(AssetReference reference) where TAsset : class => 
            GetLoadingHandle<TAsset>(reference.AssetGUID);

        public async UniTask<List<string>> GetAssetsListByLabel<TAsset>(string label) => 
            await GetAssetsListByLabel(label, typeof(TAsset));

        public void Release(AssetReference assetReference)
        {
            Addressables.Release(_handles[assetReference.AssetGUID]);
            _handles.Remove(assetReference.AssetGUID);
        }

        public void Release(AsyncOperationHandle handle) => Addressables.Release(handle);

        private async UniTask<List<string>> GetAssetsListByLabel(string label, Type type = null)
        {
            var operationHandle = Addressables.LoadResourceLocationsAsync(label, type);
            IList<IResourceLocation> locations = await operationHandle.ToUniTask();

            var assetKeys = new List<string>(locations.Count);
            foreach (IResourceLocation location in locations)
            {
                assetKeys.Add(location.PrimaryKey);
            }

            Addressables.Release(operationHandle);
            return assetKeys;
        }

        public void Cleanup()
        {
            foreach (KeyValuePair<string, AsyncOperationHandle> asyncOperationHandle in _handles)
            {
                Addressables.Release(asyncOperationHandle.Value);
            }
            _handles.Clear();
        }
    }
}