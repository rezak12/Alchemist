using Code.Infrastructure.Services.AssetProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Code.Infrastructure.Services.Factories
{
    public class PrefabByReferenceAsyncFactory<TComponent> : IFactory<AssetReferenceGameObject, UniTask<TComponent>>
    {
        private readonly IInstantiator _instantiator;
        private readonly IAssetProvider _assetProvider;

        public PrefabByReferenceAsyncFactory(IInstantiator instantiator, IAssetProvider assetProvider)
        {
            _instantiator = instantiator;
            _assetProvider = assetProvider;
        }

        public async UniTask<TComponent> Create(AssetReferenceGameObject param)
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(param);
            var component = _instantiator.InstantiatePrefabForComponent<TComponent>(prefab);
            return component;
        }
    }
}