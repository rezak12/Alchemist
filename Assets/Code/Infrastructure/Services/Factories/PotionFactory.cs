using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.ProgressServices;
using Code.Logic.Potions;
using Code.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Code.Infrastructure.Services.Factories
{
    public class PotionFactory : IPotionFactory
    {
        private readonly IPersistentProgressService _progressService;
        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;

        public PotionFactory(
            IPersistentProgressService progressService, 
            IAssetProvider assetProvider, 
            IInstantiator instantiator)
        {
            _progressService = progressService;
            _assetProvider = assetProvider;
            _instantiator = instantiator;
        }

        public async UniTask<Potion> CreatePotionAsync(PotionInfo potionInfo, Vector3 position)
        {
            PotionData potionData = await LoadPlayerPotionData();

            var potion = _instantiator.InstantiatePrefabForComponent<Potion>(
                potionData.Prefab, 
                position, 
                Quaternion.identity, 
                null);
            
            potion.Initialize(potionInfo, potionData.SFX);
            return potion;
        }
        
        private async UniTask<PotionData> LoadPlayerPotionData()
        {
            AssetReferenceT<PotionData> potionDataReference = _progressService.ChosenPotionDataReference;
            var potionData = await _assetProvider.LoadAsync<PotionData>(potionDataReference);
            
            return potionData;
        }
    }
}