using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.StaticData;
using Code.Logic.PotionMaking;
using Code.StaticData;
using Code.UI;
using Code.UI.PlayerIngredientUI;
using UnityEngine;

namespace Code.Infrastructure.Services.Factories
{
    public class UIFactory : IUIFactory
    {
        private readonly string _ingredientItemUIAddress;
        private readonly string _ingredientCharacteristicItemUIAddress;
        
        private readonly IAssetProvider _assetProvider;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticDataService;

        public UIFactory(IAssetProvider assetProvider, 
            IPersistentProgressService progressService, 
            IStaticDataService staticDataService, 
            string ingredientItemUIAddress, 
            string ingredientCharacteristicItemUIAddress)
        {
            _assetProvider = assetProvider;
            _progressService = progressService;
            _staticDataService = staticDataService;
            _ingredientItemUIAddress = ingredientItemUIAddress;
            _ingredientCharacteristicItemUIAddress = ingredientCharacteristicItemUIAddress;
        }

        public async Task<PlayerIngredientsPanel> CreatePlayerIngredientsPanelAsync(AlchemyTable alchemyTable)
        {
            WindowConfig config = _staticDataService.GetWindowByType(WindowType.PlayerIngredientsPanel);
            PlayerIngredientsPanel panelPrefab = await _assetProvider
                .LoadAsync<PlayerIngredientsPanel>(config.PrefabReference);

            var ingredientsReferences = _progressService.PlayerIngredientsAssetReferences;
            var ingredients = await _assetProvider
                .LoadAsync<IEnumerable<IngredientData>>(ingredientsReferences);

            PlayerIngredientsPanel panel = Object.Instantiate(panelPrefab);
            await panel.InitializeAsync(ingredients.ToList(), alchemyTable, this);

            return panel;
        }

        public async Task<IngredientItemUI> CreateIngredientItemUIAsync(
            IngredientData ingredient, 
            AlchemyTable alchemyTable,
            Transform parent)
        {
            var prefab = await _assetProvider.LoadAsync<IngredientItemUI>(_ingredientItemUIAddress);

            IngredientItemUI item = Object.Instantiate(prefab, parent);
            await item.InitializeAsync(
                ingredient,
                alchemyTable, 
                this);

            return item;
        }

        public async Task<IngredientCharacteristicItemUI> CreateIngredientCharacteristicItemUIAsync(
            IngredientCharacteristicAmountPair characteristicAmountPair,
            Transform parent)
        {
            var prefab = await _assetProvider.LoadAsync<IngredientCharacteristicItemUI>
                (_ingredientCharacteristicItemUIAddress);
            
            var characteristic = await _assetProvider
                .LoadAsync<PotionCharacteristic>(characteristicAmountPair.CharacteristicReference);
            
            IngredientCharacteristicItemUI item = Object.Instantiate(prefab, parent);
            item.Initialize(characteristic.Icon, characteristicAmountPair.PointsAmount);

            return item;
        }
    }
}