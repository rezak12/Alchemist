using Code.Infrastructure.Services.AssetProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Code.Infrastructure.Services.Factories
{
    public class CachePrefabFactory : IPrefabFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IAssetProvider _assetProvider;

        public CachePrefabFactory(IInstantiator instantiator, IAssetProvider assetProvider)
        {
            _instantiator = instantiator;
            _assetProvider = assetProvider;
        }
        
        public async UniTask<TComponent> CreateAsync<TComponent>(string key, Vector3 position, Transform parent = null) 
            where TComponent : MonoBehaviour
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(key);
            return _instantiator.InstantiatePrefabForComponent<TComponent>(prefab, position, Quaternion.identity, parent);
        }

        public async UniTask<TComponent> CreateAsync<TComponent>(
            AssetReference reference, 
            Vector3 position, 
            Transform parent = null)
            where TComponent : MonoBehaviour
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(reference);
            return _instantiator.InstantiatePrefabForComponent<TComponent>(prefab, position, Quaternion.identity, parent);
        }
    }
}