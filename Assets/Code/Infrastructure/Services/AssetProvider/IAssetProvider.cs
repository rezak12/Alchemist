using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.AssetProvider
{
    public interface IAssetProvider
    {
        UniTask InitializeAsync();
        UniTask<T> LoadAsync<T>(string key, bool cacheHandle = true) where T : class;
        UniTask<T> LoadAsync<T>(AssetReference assetReference, bool cacheHandle = true) where T : class;
        UniTask<T[]> LoadAsync<T>(IReadOnlyCollection<string> assetKeys, bool cacheHandle = true) where T : class;
        UniTask<T[]> LoadAsync<T>(IEnumerable<AssetReference> assetReferences, bool cacheHandle = true) where T : class;
        UniTask WarmupByLabelAsync(string label);
        UniTask<List<string>> GetAssetsListByLabel<TAsset>(string label);
        UniTask<List<string>> GetAssetsListByLabel(string label, Type type = null);
        UniTask<GameObject> InstantiateAsync(string key);
        UniTask<GameObject> InstantiateAsync(AssetReference reference);
        void Cleanup();
    }
}