using System;
using System.Linq;
using Code.Data;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.StaticData;
using Code.Logic.Orders;
using Code.Logic.PotionMaking;
using Code.Logic.Potions;
using Code.StaticData;
using Code.UI;
using Code.UI.MainMenuUI;
using Code.UI.OrderCompletedUI;
using Code.UI.PlayerIngredientsUI;
using Code.UI.PotionCharacteristicsUI;
using Code.UI.PotionMakingUI;
using Code.UI.SelectionPotionOrderUI;
using Code.UI.Store;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Services.Factories
{
    public class UIFactory : IUIFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IAssetProvider _assetProvider;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticDataService;

        public UIFactory(
            IInstantiator instantiator,
            IAssetProvider assetProvider, 
            IPersistentProgressService progressService, 
            IStaticDataService staticDataService)
        {
            _instantiator = instantiator;
            _assetProvider = assetProvider;
            _progressService = progressService;
            _staticDataService = staticDataService;
        }

        public async UniTask<SelectPotionOrderPopup> CreateSelectPotionOrderPopupAsync(
            PotionOrdersHandler potionOrdersHandler)
        {
            PopupConfig config = _staticDataService.GetPopupByType(PopupType.SelectPotionOrderPopup);
            var popupPrefab = await _assetProvider
                .LoadAsync<GameObject>(config.PrefabReference);

            var popup = _instantiator.InstantiatePrefabForComponent<SelectPotionOrderPopup>(popupPrefab);
            popup.Initialize(potionOrdersHandler);

            return popup;
        }

        public async UniTask<PotionMakingPopup> CreatePotionMakingPopup(AlchemyTable alchemyTable)
        {
            PopupConfig config = _staticDataService.GetPopupByType(PopupType.PotionMakingPopup);
            var popupPrefab = await _assetProvider
                .LoadAsync<GameObject>(config.PrefabReference);

            var ingredientsReferences = _progressService.PlayerIngredientsAssetReferences;
            var ingredients = await _assetProvider.LoadAsync<IngredientData>(ingredientsReferences);
            
            var popup = _instantiator.InstantiatePrefabForComponent<PotionMakingPopup>(popupPrefab);
            await popup.InitializeAsync(ingredients, alchemyTable);

            return popup;
        }

        public async UniTask<OrderCompletedPopup> CreateOrderCompletedPopupAsync(
            Potion result, 
            PotionOrder order,
            bool isCharacteristicsMatched)
        {
            PopupConfig config = _staticDataService.GetPopupByType(PopupType.OrderCompletedPopup);
            var popupPrefab = await _assetProvider
                .LoadAsync<GameObject>(config.PrefabReference);

            var resultCharacteristicsList = result.CharacteristicAmountPairs.ToList();
            var requirementCharacteristicsList = order.RequirementCharacteristics;
            
            var popup = _instantiator.InstantiatePrefabForComponent<OrderCompletedPopup>(popupPrefab);
            
            UniTask task;
            if (isCharacteristicsMatched)
            {
                task = popup.InitializeAsync(resultCharacteristicsList, requirementCharacteristicsList, order.Reward);
            }
            else
            {
                task = popup.InitializeAsync(resultCharacteristicsList, requirementCharacteristicsList, order.Punishment);
            }
            await task;

            return popup;
        }

        public UniTask<StorePopup> CreateStorePopupAsync()
        {
            throw new NotImplementedException();
        }

        public async UniTask<MainMenuPopup> CreateMainMenuPopupAsync()
        {
            PopupConfig config = _staticDataService.GetPopupByType(PopupType.MainMenuPopup);

            var prefab = await _assetProvider.LoadAsync<GameObject>(config.PrefabReference);
            var popup = _instantiator.InstantiatePrefabForComponent<MainMenuPopup>(prefab);
            
            popup.Initialize();
            return popup;
        }

        public async UniTask<IngredientItemUI> CreateIngredientItemUIAsync(
            IngredientData ingredient, 
            AlchemyTable alchemyTable,
            Transform parent)
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(ResourcesPaths.IngredientItemUIAddress);

            var item = _instantiator.InstantiatePrefabForComponent<IngredientItemUI>(prefab, parent);
            await item.InitializeAsync(
                ingredient,
                alchemyTable);

            return item;
        }

        public async UniTask<PotionCharacteristicItemUI> CreatePotionCharacteristicItemUIAsync(
            IngredientCharacteristicAmountPair characteristicAmountPair,
            Transform parent)
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>
                (ResourcesPaths.PotionCharacteristicItemUIAddress);
            
            var characteristic = await _assetProvider
                .LoadAsync<PotionCharacteristic>(characteristicAmountPair.CharacteristicReference);
            
            var item = _instantiator.InstantiatePrefabForComponent<PotionCharacteristicItemUI>(prefab, parent);
            item.Initialize(characteristic.Icon, characteristicAmountPair.PointsAmount);

            return item;
        }

        public async UniTask<PotionCharacteristicItemUI> CreatePotionCharacteristicItemUIAsync(
            PotionCharacteristicAmountPair characteristicAmountPair,
            Transform parent)
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>
                (ResourcesPaths.PotionCharacteristicItemUIAddress);
            
            var item = _instantiator.InstantiatePrefabForComponent<PotionCharacteristicItemUI>(prefab, parent);
            item.Initialize(characteristicAmountPair.Characteristic.Icon, characteristicAmountPair.PointsAmount);
            
            return item;
        }
    }
}