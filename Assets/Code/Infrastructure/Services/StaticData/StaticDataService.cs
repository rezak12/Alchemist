using System;
using System.Collections.Generic;
using System.Linq;
using Code.Infrastructure.Services.RandomServices;
using Code.StaticData;
using Code.UI;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService, IInitializable
    {
        private const string WindowConfigsPath = "StaticData/Windows/WindowConfigs";
        private const string OrderTypesPath = "StaticData/Orders/OrderTypes";
        private const string OrderDifficultiesPath = "StaticData/Orders/OrderDifficulties";


        private Dictionary<WindowType, WindowConfig> _windowConfigsCache;
        private PotionOrderType[] _orderTypesCache;
        private PotionOrderDifficulty[] _orderDifficultiesCache;
        
        private readonly IRandomService _randomService;

        public StaticDataService(IRandomService randomService)
        {
            _randomService = randomService;
        }
        
        void IInitializable.Initialize()
        {
            LoadWindowConfigs();
            LoadOrderTypes();
            LoadOrderDifficulties();
        }

        public WindowConfig GetWindowByType(WindowType type)
        {
            if (_windowConfigsCache.TryGetValue(type, out WindowConfig config))
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

        private void LoadWindowConfigs()
        {
            _windowConfigsCache = Resources
                .LoadAll<WindowConfig>(WindowConfigsPath)
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
    }
}