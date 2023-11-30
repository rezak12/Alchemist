using Code.Infrastructure.Services.AssetProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.Factories
{
    public class NonCachePrefabFactory : IPrefabFactory
    {
        private readonly IAssetProvider _assetProvider;

        public NonCachePrefabFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        
        public async UniTask<TComponent> Create<TComponent>(string key) where TComponent : MonoBehaviour
        {
            GameObject gameObject = await _assetProvider.InstantiateAsync(key);
            return gameObject.GetComponent<TComponent>();
        }

        public async UniTask<TComponent> Create<TComponent>(AssetReference reference) where TComponent : MonoBehaviour
        {
            GameObject gameObject = await _assetProvider.InstantiateAsync(reference);
            return gameObject.GetComponent<TComponent>();
        }
    }
}