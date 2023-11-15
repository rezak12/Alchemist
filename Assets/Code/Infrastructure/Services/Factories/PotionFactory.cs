using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.ProgressServices;
using Code.Logic.Potions;
using Code.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.Factories
{
    public class PotionFactory : IPotionFactory
    {
        private readonly IPersistentProgressService _progressService;
        private readonly IAssetProvider _assetProvider;

        public PotionFactory(IPersistentProgressService progressService, IAssetProvider assetProvider)
        {
            _progressService = progressService;
            _assetProvider = assetProvider;
        }

        public async UniTask<Potion> CreatePotionAsync(PotionInfo potionInfo, Vector3 position)
        {
            Potion potionPrefab = await LoadPlayerPotionPrefab();

            Potion potion = Object.Instantiate(potionPrefab, position, Quaternion.identity);
            potion.Initialize(potionInfo);
            
            return potion;
        }
        
        private async UniTask<Potion> LoadPlayerPotionPrefab()
        {
            var potionDataReference = _progressService.ChosenPotionDataReference;
            var potionData = await _assetProvider.LoadAsync<PotionData>(potionDataReference);
            
            return potionData.Prefab;
        }
    }
}