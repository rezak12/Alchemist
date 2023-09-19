using System.Threading.Tasks;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.ProgressServices;
using Code.Logic.Potions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.Factories
{
    public class PotionFactory : IPotionFactory
    {
        private IPersistentProgressService _progressService;
        private IAssetProvider _assetProvider;

        public async Task<Potion> CreatePotionAsync(PotionInfo potionInfo, Vector3 position)
        {
            Potion potionPrefab = await LoadPlayerPotionPrefab();

            Potion potion = Object.Instantiate(potionPrefab, position, Quaternion.identity);
            potion.Initialize(potionInfo);
            
            return potion;
        }
        
        private async Task<Potion> LoadPlayerPotionPrefab()
        {
            AssetReferenceT<Potion> potionPrefabReference = _progressService.CurrentPlayerPotionPrefabReference;
            var potionPrefab = await _assetProvider.LoadAsync<Potion>(potionPrefabReference);
            return potionPrefab;
        }
    }
}