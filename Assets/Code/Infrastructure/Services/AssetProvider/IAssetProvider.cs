using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

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
        UniTask WarmupByLabelAsync(string label);
        UniTask<List<string>> GetAssetsListByLabel<TAsset>(string label);
        void Cleanup();
    }
}