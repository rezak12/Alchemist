using System;
using System.Collections.Generic;
using System.Linq;
using Code.Infrastructure.Services.RandomServices;
using Code.StaticData;
using Code.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string WindowConfigsPath = "StaticData/Windows/WindowConfigs";
        private const string OrderTypesPath = "StaticData/Orders/OrderTypes";
        private const string OrderDifficultiesPath = "StaticData/Orders/OrderDifficulties";
        private const string LevelConfigsPath = "StaticData/LevelConfigs";

        private Dictionary<PopupType, PopupConfig> _popupConfigsCache;
        private PotionOrderType[] _orderTypesCache;
        private PotionOrderDifficulty[] _orderDifficultiesCache;
        private Dictionary<string,LevelConfig> _levelConfigsCache;

        private readonly IRandomService _randomService;

        private UniTaskCompletionSource _taskCompletionSource;
        
        public StaticDataService(IRandomService randomService)
        {
            _randomService = randomService;
        }

        public UniTask InitializeAsync()
        {
            _taskCompletionSource = new UniTaskCompletionSource();
            LoadPopupConfigs();
            LoadOrderTypes();
            LoadOrderDifficulties();
            LoadLevelConfigs();
            return _taskCompletionSource.Task;
        }

        public PopupConfig GetPopupByType(PopupType type)
        {
            if (_popupConfigsCache.TryGetValue(type, out PopupConfig config))
            {
                return config;
            }
            throw new NullReferenceException();
        }

        public LevelConfig GetLevelConfigBySceneName(string sceneName)
        {
            if (_levelConfigsCache.TryGetValue(sceneName, out LevelConfig config))
            {
                return config;
            }
            throw new NullReferenceException();
        }

        public PotionOrderType GetRandomPotionOrderType()
        {
            return _orderTypesCache[_randomService.Next(0, _orderTypesCache.Length)];
        }

        public PotionOrderDifficulty GetRandomPotionOrderDifficulty()
        { 
            return _orderDifficultiesCache[_randomService.Next(0, _orderDifficultiesCache.Length)];
        }

        private void LoadPopupConfigs()
        {
            _popupConfigsCache = Resources
                .LoadAll<PopupConfig>(WindowConfigsPath)
                .ToDictionary(config => config.Type, config => config);
        }

        private void LoadOrderTypes()
        {
            _orderTypesCache = Resources.LoadAll<PotionOrderType>(OrderTypesPath);
        }

        private void LoadOrderDifficulties()
        {
            _orderDifficultiesCache = Resources.LoadAll<PotionOrderDifficulty>(OrderDifficultiesPath);
        }

        private void LoadLevelConfigs()
        {
            _levelConfigsCache = Resources
                .LoadAll<LevelConfig>(LevelConfigsPath)
                .ToDictionary(config => config.SceneName, config => config);
        }
    }
}