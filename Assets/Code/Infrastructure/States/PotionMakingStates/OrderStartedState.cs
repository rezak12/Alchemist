using System.Collections.Generic;
using System.Linq;
using Code.Data;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.StaticData;
using Code.Logic.Orders;
using Code.Logic.PotionMaking;
using Code.StaticData;
using Code.StaticData.Configs;
using Code.StaticData.Ingredients;
using Code.StaticData.Potions;
using Code.UI.AwaitingOverlays;
using Code.UI.PotionMakingUI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.States.PotionMakingStates
{
    public class OrderStartedState : IPayloadState<PotionOrder>
    {
        private GameObject _environmentPrefab;
        private PotionMakingPopup _potionMakingPopup;
        private AlchemyTableComponent _alchemyTable;
        
        private readonly SelectedPotionOrderHolder _selectedOrderHolder;
        private readonly IStaticDataService _staticDataService;
        private readonly IAlchemyTableFactory _tableFactory;
        private readonly IEnvironmentFactory _environmentFactory;
        private readonly IUIFactory _uiFactory;
        private readonly IAwaitingOverlay _awaitingOverlay;
        private readonly IAssetProvider _assetProvider;
        private readonly IPersistentProgressService _progressService;

        public OrderStartedState(IStaticDataService staticDataService, 
            IAlchemyTableFactory tableFactory,
            IUIFactory uiFactory,
            IAwaitingOverlay awaitingOverlay,
            SelectedPotionOrderHolder selectedOrderHolder, 
            IAssetProvider assetProvider, 
            IPersistentProgressService progressService, 
            IEnvironmentFactory environmentFactory)
        {
            _uiFactory = uiFactory;
            _tableFactory = tableFactory;
            _staticDataService = staticDataService;
            _awaitingOverlay = awaitingOverlay;
            _selectedOrderHolder = selectedOrderHolder;
            _assetProvider = assetProvider;
            _progressService = progressService;
            _environmentFactory = environmentFactory;
        }
        public async UniTask Enter(PotionOrder payload)
        {
            await WarmupAssets();
            
            _selectedOrderHolder.PutOrder(payload);
            LevelConfig levelConfig = _staticDataService.GetLevelConfigBySceneName(ResourcesAddresses.PotionMakingSceneAddress);

            _environmentPrefab = await _environmentFactory.CreateEnvironmentAsync(levelConfig.EnvironmentPosition);
            _alchemyTable = await _tableFactory.CreateTableAsync(levelConfig.TablePosition);
            _potionMakingPopup = await _uiFactory.CreatePotionMakingPopupAsync(_alchemyTable);
           
            _awaitingOverlay.Hide().Forget();
        }

        public UniTask Exit()
        {
            Object.Destroy(_environmentPrefab);
            Object.Destroy(_alchemyTable.gameObject);
            Object.Destroy(_potionMakingPopup.gameObject);
            return UniTask.CompletedTask;
        }

        private async UniTask WarmupAssets()
        {
            await UniTask.WhenAll(
                WarmupPotionData(),
                WarmupIngredients());
        }

        private async UniTask WarmupPotionData()
        {
            var potionData = await _assetProvider.LoadAsync<PotionData>(_progressService.ChosenPotionDataReference);
            await _assetProvider.LoadAsync<AudioClip>(potionData.SFX);
        }

        private async UniTask WarmupIngredients()
        {
            IngredientData[] ingredients = await _assetProvider
                .LoadAsync<IngredientData>(_progressService.OwnedIngredientsAssetReferences);
            
            IEnumerable<AssetReference> dependencies = GetDependenciesFromIngredients(ingredients);
            await _assetProvider.LoadAsync<object>(dependencies);
        }

        private static IEnumerable<AssetReference> GetDependenciesFromIngredients(IReadOnlyCollection<IngredientData> ingredients)
        {
            var dependencies = new List<AssetReference>(ingredients.Count * 3);
            foreach (IngredientData ingredientData in ingredients)
            {
                dependencies.Add(ingredientData.PrefabReference);
                dependencies.Add(ingredientData.AudioClipReference);
                dependencies.AddRange(
                    ingredientData.CharacteristicAmountPairs
                        .Select(pair => pair.CharacteristicReference));
            }

            return dependencies;
        }
    }
}