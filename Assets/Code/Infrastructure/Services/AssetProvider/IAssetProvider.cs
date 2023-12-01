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
        UniTask<T> LoadAsync<T>(string key) where T : class;
        UniTask<T> LoadAsync<T>(AssetReference assetReference) where T : class;
        UniTask<T[]> LoadAsync<T>(IEnumerable<string> assetKeys) where T : class;
        UniTask<T[]> LoadAsync<T>(IEnumerable<AssetReference> assetReferences) where T : class;
        UniTask WarmupByLabelAsync(string label);
        UniTask<GameObject> InstantiateAsync(string key);
        UniTask<GameObject> InstantiateAsync(AssetReference reference);
        void Cleanup();
    }
}