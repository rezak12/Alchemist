using Code.Animations;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.Factories
{
    public interface IIngredientFactory
    {
        UniTask<IngredientAnimator> CreateIngredientAsync(
            AssetReferenceGameObject ingredientDataPrefabReference,
            Vector3 position);
    }
}