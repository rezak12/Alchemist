using System.Threading.Tasks;
using Code.Logic.Orders;
using Code.Logic.PotionMaking;
using Code.Logic.Potions;
using Code.StaticData;
using Code.UI.OrdersViewUI;
using Code.UI.PlayerIngredientsUI;
using Code.UI.Store;
using UnityEngine;

namespace Code.Infrastructure.Services.Factories
{
    public interface IUIFactory
    {
        Task<SelectPotionOrderWindow> CreateSelectPotionOrderPopup(PotionOrdersHandler potionOrdersHandler);
        Task<PlayerIngredientsPanel> CreatePlayerIngredientsPanelAsync(AlchemyTable alchemyTable);
        
        Task<StoreWindow> CreateStoreWindow();

        Task<IngredientItemUI> CreateIngredientItemUIAsync(
            IngredientData ingredient, 
            AlchemyTable alchemyTable, 
            Transform parent);

        Task<PotionCharacteristicItemUI> CreatePotionCharacteristicItemUIAsync(
            IngredientCharacteristicAmountPair characteristicAmountPair,
            Transform parent);

        Task<PotionCharacteristicItemUI> CreatePotionCharacteristicItemUIAsync(
            PotionCharacteristicAmountPair characteristicAmountPair,
            Transform parent);
    }
}