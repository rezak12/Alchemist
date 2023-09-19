using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.ProgressServices;
using Code.Logic.Potions;
using Code.StaticData;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.Factories
{
    public class PotionInfoFactory : IPotionInfoFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IPersistentProgressService _progressService;

        public PotionInfoFactory(IAssetProvider assetProvider, IPersistentProgressService progressService)
        {
            _assetProvider = assetProvider;
            _progressService = progressService;
        }

        public async Task<PotionInfo> CreatePotionInfoAsync(IEnumerable<IngredientData> ingredients)
        {
            var characteristics = await CalculatePotionCharacteristicsAsync(ingredients);
            return new PotionInfo(characteristics);
        }
        
        private async Task<PotionCharacteristicAmountPair[]> CalculatePotionCharacteristicsAsync
            (IEnumerable<IngredientData> ingredients)
        {
            var groupedCharacteristics = GroupCharacteristics(ingredients);
            var characteristicAmountPairs =
                await CreateCharacteristicAmountPairsAsync(groupedCharacteristics);

            return characteristicAmountPairs;
        }

        private Dictionary<AssetReferenceT<PotionCharacteristic>, int> GroupCharacteristics(
            IEnumerable<IngredientData> ingredients)
        {
            var groupedCharacteristics = new Dictionary<AssetReferenceT<PotionCharacteristic>, int>();

            foreach (IngredientData ingredient in ingredients)
            {
                foreach (IngredientCharacteristicAmountPair pair in ingredient.CharacteristicAmountPairs)
                {
                    var characteristicReference = pair.CharacteristicReference;
                    var pointsAmount = pair.PointsAmount;

                    if (groupedCharacteristics.ContainsKey(characteristicReference))
                    {
                        groupedCharacteristics[characteristicReference] += pointsAmount;
                    }
                    else
                    {
                        groupedCharacteristics.Add(characteristicReference, pointsAmount);
                    }
                }
            }

            return groupedCharacteristics;
        }

        private async Task<PotionCharacteristicAmountPair[]> CreateCharacteristicAmountPairsAsync(
            Dictionary<AssetReferenceT<PotionCharacteristic>, int> groupedCharacteristics)
        {
            var tasks = groupedCharacteristics.Select(async pair =>
            {
                var characteristic = await _assetProvider.LoadAsync<PotionCharacteristic>(pair.Key);
                return new PotionCharacteristicAmountPair(characteristic, pair.Value);
            });

            return await Task.WhenAll(tasks);
        }
    }
}