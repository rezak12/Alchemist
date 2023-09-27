using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.AssetProvider
{
    public interface IAssetProvider
    {
        UniTask InitializeAsync();
        UniTask<T> LoadAsync<T>(AssetReference assetReference) where T : class;
        UniTask<T> LoadAsync<T>(string address) where T : class;
        UniTask<T[]> LoadAsync<T>(IEnumerable<AssetReference> assetReferences) where T : class;
    }
}