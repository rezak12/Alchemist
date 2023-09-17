using System;
using System.Linq;
using System.Threading.Tasks;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.StaticData;
using Code.Logic.Orders;
using Code.Logic.PotionMaking;
using Code.Logic.Potions;
using Code.StaticData;
using Code.UI;
using Code.UI.OrderCompletedUI;
using Code.UI.OrdersViewUI;
using Code.UI.PlayerIngredientsUI;
using Code.UI.Store;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Object = UnityEngine.Object;

namespace Code.Infrastructure.Services.Factories
{
    public class UIFactory : IUIFactory
    {
        private readonly AssetReference _ingredientItemUIReference;
        private readonly AssetReference _ingredientCharacteristicItemUIReference;
        private readonly IInstantiator _instantiator;
        private readonly IAssetProvider _assetProvider;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticDataService;

        public UIFactory(
            IInstantiator instantiator,
            IAssetProvider assetProvider, 
            IPersistentProgressService progressService, 
            IStaticDataService staticDataService, 
            AssetReference ingredientItemUIReference, 
            AssetReference ingredientCharacteristicItemUIReference)
        {
            _instantiator = instantiator;
            _assetProvider = assetProvider;
            _progressService = progressService;
            _staticDataService = staticDataService;
            _ingredientItemUIReference = ingredientItemUIReference;
            _ingredientCharacteristicItemUIReference = ingredientCharacteristicItemUIReference;
        }

        public async Task<SelectPotionOrderPopup> CreateSelectPotionOrderPopupAsync(
            PotionOrdersHandler potionOrdersHandler,
            ChosenPotionOrderSender potionOrdersSender)
        {
            WindowConfig config = _staticDataService.GetWindowByType(WindowType.SelectPotionOrderPopup);
            var panelPrefab = await _assetProvider
                .LoadAsync<SelectPotionOrderPopup>(config.PrefabReference);

            var prefab = _instantiator.InstantiatePrefabForComponent<SelectPotionOrderPopup>(panelPrefab);
            prefab.Initialize(potionOrdersHandler, potionOrdersSender);

            return prefab;
        }

        public async Task<PlayerIngredientsPanel> CreatePlayerIngredientsPanelAsync(AlchemyTable alchemyTable)
        {
            WindowConfig config = _staticDataService.GetWindowByType(WindowType.PlayerIngredientsPanel);
            var panelPrefab = await _assetProvider
                .LoadAsync<PlayerIngredientsPanel>(config.PrefabReference);

            var ingredientsReferences = _progressService.PlayerIngredientsAssetReferences;

            var ingredients = await _assetProvider.LoadAsync<IngredientData>(ingredientsReferences);
            
            PlayerIngredientsPanel panel = Object.Instantiate(panelPrefab);
            await panel.InitializeAsync(ingredients, alchemyTable, this);

            return panel;
        }

        public async Task<OrderCompletedPopup> CreateOrderCompletedPopupAsync(
            Potion result, 
            PotionOrder order,
            bool isCharacteristicsMatched)
        {
            WindowConfig config = _staticDataService.GetWindowByType(WindowType.OrderCompletedPopup);
            var prefab = await _assetProvider
                .LoadAsync<OrderCompletedPopup>(config.PrefabReference);

            var resultCharacteristicsList = result.CharacteristicAmountPairs.ToList();
            var requirementCharacteristicsList = order.RequirementCharacteristics;
            
            var popup = _instantiator.InstantiatePrefabForComponent<OrderCompletedPopup>(prefab);
            
            Task task;
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

        public Task<StoreWindow> CreateStoreWindow()
        {
            throw new NotImplementedException();
        }

        public async Task<IngredientItemUI> CreateIngredientItemUIAsync(
            IngredientData ingredient, 
            AlchemyTable alchemyTable,
            Transform parent)
        {
            var prefab = await _assetProvider.LoadAsync<IngredientItemUI>(_ingredientItemUIReference);

            IngredientItemUI item = Object.Instantiate(prefab, parent);
            await item.InitializeAsync(
                ingredient,
                alchemyTable, 
                this);

            return item;
        }

        public async Task<PotionCharacteristicItemUI> CreatePotionCharacteristicItemUIAsync(
            IngredientCharacteristicAmountPair characteristicAmountPair,
            Transform parent)
        {
            var prefab = await _assetProvider.LoadAsync<PotionCharacteristicItemUI>
                (_ingredientCharacteristicItemUIReference);
            
            var characteristic = await _assetProvider
                .LoadAsync<PotionCharacteristic>(characteristicAmountPair.CharacteristicReference);
            
            PotionCharacteristicItemUI item = Object.Instantiate(prefab, parent);
            item.Initialize(characteristic.Icon, characteristicAmountPair.PointsAmount);

            return item;
        }

        public async Task<PotionCharacteristicItemUI> CreatePotionCharacteristicItemUIAsync(
            PotionCharacteristicAmountPair characteristicAmountPair,
            Transform parent)
        {
            var prefab = await _assetProvider.LoadAsync<PotionCharacteristicItemUI>
                (_ingredientCharacteristicItemUIReference);
            
            PotionCharacteristicItemUI item = Object.Instantiate(prefab, parent);
            item.Initialize(characteristicAmountPair.Characteristic.Icon, characteristicAmountPair.PointsAmount);
            
            return item;
        }
    }
}