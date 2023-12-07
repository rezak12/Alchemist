using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.ProgressServices;
using Code.Logic.PotionMaking;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Services.Factories
{
    public class AlchemyTableFactory : IAlchemyTableFactory
    {
        private readonly IPersistentProgressService _progressService;
        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;

        public AlchemyTableFactory(
            IPersistentProgressService progressService, 
            IAssetProvider assetProvider, 
            IInstantiator instantiator)
        {
            _instantiator = instantiator;
            _assetProvider = assetProvider;
            _progressService = progressService;
        }
        
        public async UniTask<AlchemyTableComponent> CreateTableAsync(Vector3 position)
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(_progressService.ChosenAlchemyTablePrefabReference);
            
            var table = _instantiator.InstantiatePrefabForComponent<AlchemyTableComponent>(
                prefab, 
                position, 
                Quaternion.identity, 
                null);
            
            table.Initialize();
            return table;
        }
    }
}