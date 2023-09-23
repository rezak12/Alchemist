using System.Threading.Tasks;
using Code.Animations;
using Code.Infrastructure.Services.AssetProvider;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.Factories
{
    public class IngredientFactory : IIngredientFactory
    {
        private readonly IAssetProvider _assetProvider;

        public IngredientFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async Task<IngredientAnimator> CreateIngredientAsync(
            AssetReferenceGameObject ingredientDataPrefabReference, Vector3 position)
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(ingredientDataPrefabReference);
            GameObject ingredientGameObject = Object.Instantiate(prefab, position, Quaternion.identity);
            
            return ingredientGameObject.GetComponent<IngredientAnimator>();
        }
    }
}