using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.ProgressServices;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Services.Factories
{
    public class EnvironmentFactory : IEnvironmentFactory
    {
        private readonly IPersistentProgressService _progressService;
        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;

        public EnvironmentFactory(
            IPersistentProgressService progressService, 
            IAssetProvider assetProvider, 
            IInstantiator instantiator)
        {
            _instantiator = instantiator;
            _assetProvider = assetProvider;
            _progressService = progressService;
        }
        
        public async UniTask<GameObject> CreateEnvironmentAsync(Vector3 position)
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(_progressService.ChosenEnvironmentPrefabReference);
            
            GameObject environment = _instantiator.InstantiatePrefab(
                prefab, 
                position, 
                Quaternion.identity, 
                null);
            
            return environment;
        }
    }
}