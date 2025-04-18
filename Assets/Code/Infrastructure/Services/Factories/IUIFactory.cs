﻿using System.Threading.Tasks;
using Code.Logic.Orders;
using Code.Logic.PotionMaking;
using Code.Logic.Potions;
using Code.StaticData;
using Code.StaticData.Ingredients;
using Code.UI.MainMenuUI;
using Code.UI.OrderCompletedUI;
using Code.UI.PlayerIngredientsUI;
using Code.UI.PotionCharacteristicsUI;
using Code.UI.PotionMakingUI;
using Code.UI.SelectionPotionOrderUI;
using Code.UI.SettingsScreen;
using Code.UI.Store;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.Factories
{
    public interface IUIFactory
    {
        UniTask<SelectPotionOrderPopup> CreatePotionOrderSelectionPopupAsync(
            PotionOrdersHandler potionOrdersHandler);
        
        UniTask<PotionMakingPopup> CreatePotionMakingPopupAsync(AlchemyTableComponent alchemyTable);

        UniTask<OrderCompletedPopup> CreateOrderCompletedPopupAsync(
            Potion result, 
            PotionOrder order, 
            bool isCharacteristicsMatched);

        UniTask<ShopPopup> CreateStorePopupAsync();
        
        UniTask<MainMenuPopup> CreateMainMenuPopupAsync();

        UniTask<SettingsPopup> CreateSettingsScreen();

        UniTask<IngredientItemUI> CreateIngredientItemUIAsync(
            IngredientData ingredient, 
            AlchemyTableComponent alchemyTable, 
            Transform parent);

        UniTask<PotionCharacteristicItemUI> CreatePotionCharacteristicItemUIAsync(
            IngredientCharacteristicAmountPair characteristicAmountPair,
            Transform parent);

        UniTask<PotionCharacteristicItemUI> CreatePotionCharacteristicItemUIAsync(
            PotionCharacteristicAmountPair characteristicAmountPair,
            Transform parent);
    }
}