using Code.Infrastructure.Services.AssetProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Code.Infrastructure.Services.Factories
{
    public class PrefabByAddressAsyncFactory<TComponent> : IFactory<string, UniTask<TComponent>>
    {
        private readonly IInstantiator _instantiator;
        private readonly IAssetProvider _assetProvider;

        public PrefabByAddressAsyncFactory(IInstantiator instantiator, IAssetProvider assetProvider)
        {
            _instantiator = instantiator;
            _assetProvider = assetProvider;
        }

        public async UniTask<TComponent> Create(string assetKey)
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(assetKey);
            var component = _instantiator.InstantiatePrefabForComponent<TComponent>(prefab);
            return component;
        }
    }
}