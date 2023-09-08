using System.Threading.Tasks;
using Code.Animations;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.Factories
{
    public interface IIngredientFactory
    {
        Task<IngredientAnimator> CreateIngredientAsync(
            AssetReferenceT<IngredientAnimator> ingredientDataPrefabReference,
            Vector3 position);
    }
}