using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Code.Infrastructure.Services.AssetProvider
{
    public interface IAssetProvider
    {
        UniTask InitializeAsync();
        UniTask<TAsset> LoadAsync<TAsset>(string key, bool cacheHandle = true) where TAsset : class;
        UniTask<TAsset> LoadAsync<TAsset>(AssetReference assetReference, bool cacheHandle = true) where TAsset : class;
        UniTask<TAsset[]> LoadAsync<TAsset>(IReadOnlyCollection<string> assetKeys, bool cacheHandle = true) 
            where TAsset : class;
        UniTask<TAsset[]> LoadAsync<TAsset>(IEnumerable<AssetReference> assetReferences, bool cacheHandle = true) 
            where TAsset : class;
        AsyncOperationHandle<TAsset> GetLoadingHandle<TAsset>(string key) where TAsset : class;
        AsyncOperationHandle<TAsset> GetLoadingHandle<TAsset>(AssetReference reference) where TAsset : class;
        UniTask<List<string>> GetAssetsListByLabel<TAsset>(string label);
        void Release(AssetReference assetReference);
        void Release(AsyncOperationHandle handle);
        void Cleanup();
    }
}