using Code.Animations;
using Code.Infrastructure.Services.AssetProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Code.Infrastructure.Services.Factories
{
    public class IngredientFactory : IIngredientFactory
    {
        private readonly IAssetProvider _assetProvider;
        private IInstantiator _instantiator;

        public IngredientFactory(IAssetProvider assetProvider, IInstantiator instantiator)
        {
            _assetProvider = assetProvider;
            _instantiator = instantiator;
        }

        public async UniTask<IngredientTweener> CreateIngredientAsync(
            AssetReferenceGameObject ingredientDataPrefabReference, 
            Vector3 position)
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(ingredientDataPrefabReference);
            return _instantiator.InstantiatePrefabForComponent<IngredientTweener>(
                prefab, 
                position, 
                Quaternion.identity, 
                null);
        }
    }
}