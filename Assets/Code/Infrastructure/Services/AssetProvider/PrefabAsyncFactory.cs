using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Services.AssetProvider
{
    public class PrefabAsyncFactory<TComponent> : IFactory<string, UniTask<TComponent>>
    {
        private readonly IInstantiator _instantiator;
        private readonly IAssetProvider _assetProvider;

        public PrefabAsyncFactory(IInstantiator instantiator, IAssetProvider assetProvider)
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