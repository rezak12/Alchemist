using System.Threading.Tasks;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.StaticData;
using Code.Logic.PotionMaking;
using Code.StaticData;
using Code.UI;
using Code.UI.PlayerIngredientsUI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.Factories
{
    public class UIFactory : IUIFactory
    {
        private readonly AssetReference _ingredientItemUIReference;
        private readonly AssetReference _ingredientCharacteristicItemUIReference;
        
        private readonly IAssetProvider _assetProvider;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticDataService;

        public UIFactory(IAssetProvider assetProvider, 
            IPersistentProgressService progressService, 
            IStaticDataService staticDataService, 
            AssetReference ingredientItemUIReference, 
            AssetReference ingredientCharacteristicItemUIReference)
        {
            _assetProvider = assetProvider;
            _progressService = progressService;
            _staticDataService = staticDataService;
            _ingredientItemUIReference = ingredientItemUIReference;
            _ingredientCharacteristicItemUIReference = ingredientCharacteristicItemUIReference;
        }

        public async Task<PlayerIngredientsPanel> CreatePlayerIngredientsPanelAsync(AlchemyTable alchemyTable)
        {
            WindowConfig config = _staticDataService.GetWindowByType(WindowType.PlayerIngredientsPanel);
            PlayerIngredientsPanel panelPrefab = await _assetProvider
                .LoadAsync<PlayerIngredientsPanel>(config.PrefabReference);

            var ingredientsReferences = _progressService.PlayerIngredientsAssetReferences;

            var ingredients = await _assetProvider.LoadAsync<IngredientData>(ingredientsReferences);
            
            PlayerIngredientsPanel panel = Object.Instantiate(panelPrefab);
            await panel.InitializeAsync(ingredients, alchemyTable, this);

            return panel;
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

        public async Task<IngredientCharacteristicItemUI> CreateIngredientCharacteristicItemUIAsync(
            IngredientCharacteristicAmountPair characteristicAmountPair,
            Transform parent)
        {
            var prefab = await _assetProvider.LoadAsync<IngredientCharacteristicItemUI>
                (_ingredientCharacteristicItemUIReference);
            
            var characteristic = await _assetProvider
                .LoadAsync<PotionCharacteristic>(characteristicAmountPair.CharacteristicReference);
            
            IngredientCharacteristicItemUI item = Object.Instantiate(prefab, parent);
            item.Initialize(characteristic.Icon, characteristicAmountPair.PointsAmount);

            return item;
        }
    }
}