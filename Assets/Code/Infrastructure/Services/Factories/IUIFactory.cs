using Code.Logic.Orders;
using Code.Logic.PotionMaking;
using Code.Logic.Potions;
using Code.StaticData;
using Code.UI.MainMenuUI;
using Code.UI.OrderCompletedUI;
using Code.UI.OrdersViewUI;
using Code.UI.PlayerIngredientsUI;
using Code.UI.PotionCharacteristicsUI;
using Code.UI.PotionMakingUI;
using Code.UI.Store;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.Factories
{
    public interface IUIFactory
    {
        UniTask<SelectPotionOrderPopup> CreateSelectPotionOrderPopupAsync(
            PotionOrdersHandler potionOrdersHandler);
        UniTask<PotionMakingPopup> CreatePotionMakingPopup(AlchemyTable alchemyTable);

        UniTask<OrderCompletedPopup> CreateOrderCompletedPopupAsync(
            Potion result, 
            PotionOrder order, 
            bool isCharacteristicsMatched);

        UniTask<StorePopup> CreateStorePopupAsync();
        
        UniTask<MainMenuPopup> CreateMainMenuPopupAsync();

        UniTask<IngredientItemUI> CreateIngredientItemUIAsync(
            IngredientData ingredient, 
            AlchemyTable alchemyTable, 
            Transform parent);

        UniTask<PotionCharacteristicItemUI> CreatePotionCharacteristicItemUIAsync(
            IngredientCharacteristicAmountPair characteristicAmountPair,
            Transform parent);

        UniTask<PotionCharacteristicItemUI> CreatePotionCharacteristicItemUIAsync(
            PotionCharacteristicAmountPair characteristicAmountPair,
            Transform parent);
    }
}