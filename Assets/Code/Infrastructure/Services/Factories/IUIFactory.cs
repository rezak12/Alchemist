using System.Threading.Tasks;
using Code.Logic.PotionMaking;
using Code.StaticData;
using Code.UI.PlayerIngredientUI;
using UnityEngine;

namespace Code.Infrastructure.Services.Factories
{
    public interface IUIFactory
    {
        Task<PlayerIngredientsPanel> CreatePlayerIngredientsPanelAsync(AlchemyTable alchemyTable);
        Task<IngredientItemUI> CreateIngredientItemUIAsync(IngredientData ingredient, AlchemyTable alchemyTable, Transform parent);
        Task<IngredientCharacteristicItemUI> CreateIngredientCharacteristicItemUIAsync(
            IngredientCharacteristicAmountPair characteristicAmountPair,
            Transform parent);
    }
}