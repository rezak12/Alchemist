using System.Collections.Generic;
using System.Linq;
using Code.Data;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.Pool;
using Code.Infrastructure.Services.RandomServices;
using Code.StaticData;
using Code.UI;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<PopupType, PopupConfig> _popupConfigsCache;
        private Dictionary<string,LevelConfig> _levelConfigsCache;
        private Dictionary<PoolObjectType, PoolObjectConfig> _poolObjectConfigsCache;
        private PotionOrderType[] _orderTypesCache;
        private PotionOrderDifficulty[] _orderDifficultiesCache;
        private AmbientReferencesCatalog _ambientReferencesCatalog;

        private readonly IRandomService _randomService;
        private readonly IAssetProvider _assetProvider;

        public StaticDataService(IRandomService randomService, IAssetProvider assetProvider)
        {
            _randomService = randomService;
            _assetProvider = assetProvider;
        }

        public async UniTask InitializeAsync()
        {
            await UniTask.WhenAll(
                LoadPopupConfigs(),
                LoadLevelConfigs(),
                LoadPoolConfigs(),
                LoadOrderTypes(),
                LoadOrderDifficulties(),
                LoadAmbientReferencesCatalog());
        }

        public PopupConfig GetPopupByType(PopupType type) => _popupConfigsCache[type];

        public LevelConfig GetLevelConfigBySceneName(string sceneName) => _levelConfigsCache[sceneName];

        public PoolObjectConfig GetPoolConfigByType(PoolObjectType type) => _poolObjectConfigsCache[type];

        public PotionOrderType GetRandomPotionOrderType() =>
            _orderTypesCache[_randomService.Next(0, _orderTypesCache.Length)];

        public PotionOrderDifficulty GetRandomPotionOrderDifficulty() =>
            _orderDifficultiesCache[_randomService.Next(0, _orderDifficultiesCache.Length)];

        public AmbientReferencesCatalog GetAmbientReferencesCatalog() => _ambientReferencesCatalog;

        private async UniTask LoadPopupConfigs()
        {
            PopupConfig[] configsList = await LoadConfigs<PopupConfig>();
            _popupConfigsCache = configsList.ToDictionary(config => config.Type, config => config);
        }

        private async UniTask LoadLevelConfigs()
        {
            LevelConfig[] configsList = await LoadConfigs<LevelConfig>();
            _levelConfigsCache = configsList.ToDictionary(config => config.SceneName, config => config);
        }

        private async UniTask LoadPoolConfigs()
        {
            PoolObjectConfig[] configsList = await LoadConfigs<PoolObjectConfig>();
            _poolObjectConfigsCache = configsList.ToDictionary(config => config.Type, config => config);
        }

        private async UniTask LoadOrderTypes() =>
            _orderTypesCache = await LoadConfigs<PotionOrderType>();

        private async UniTask LoadOrderDifficulties() =>
            _orderDifficultiesCache = await LoadConfigs<PotionOrderDifficulty>();
        
        private async UniTask LoadAmbientReferencesCatalog()
        {
            AmbientReferencesCatalog[] loadedConfigArray = await LoadConfigs<AmbientReferencesCatalog>();
            _ambientReferencesCatalog = loadedConfigArray.First();
        }

        private async UniTask<TConfig[]> LoadConfigs<TConfig>() where TConfig : class
        {
            List<string> keys = await GetConfigKeys<TConfig>();
            return await _assetProvider.LoadAsync<TConfig>(keys, cacheHandle: false);
        }

        private async UniTask<List<string>> GetConfigKeys<TConfig>() => 
            await _assetProvider.GetAssetsListByLabel<TConfig>(ResourcesLabels.Configs);
    }
}