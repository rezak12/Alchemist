using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.AssetProvider
{
    public interface IAssetProvider
    {
        Task<T> LoadAsync<T>(AssetReference assetReference) where T : class;
        Task<T> LoadAsync<T>(string address) where T : class;
        Task<T[]> LoadAsync<T>(IEnumerable<AssetReference> assetReferences) where T : class;
    }
}