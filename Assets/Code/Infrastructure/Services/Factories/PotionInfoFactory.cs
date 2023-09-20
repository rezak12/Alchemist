using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Infrastructure.Services.AssetProvider;
using Code.Logic.Potions;
using Code.StaticData;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.Factories
{
    public class PotionInfoFactory : IPotionInfoFactory
    {
        private readonly IAssetProvider _assetProvider;

        public PotionInfoFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
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
            return await CreateCharacteristicAmountPairsAsync(groupedCharacteristics);;
        }

        private Dictionary<AssetReferenceT<PotionCharacteristic>, int> GroupCharacteristics(
            IEnumerable<IngredientData> ingredients)
        {
            var characteristicGuidAmountPairs = new Dictionary<string, int>();
            foreach (IngredientData ingredient in ingredients)
            {
                foreach (IngredientCharacteristicAmountPair pair in ingredient.CharacteristicAmountPairs)
                {
                    var characteristicGuid = pair.CharacteristicReference.AssetGUID;
                    var pointsAmount = pair.PointsAmount;

                    if (characteristicGuidAmountPairs.ContainsKey(characteristicGuid))
                    {
                        characteristicGuidAmountPairs[characteristicGuid] += pointsAmount;
                    }
                    else
                    {
                        characteristicGuidAmountPairs.Add(characteristicGuid, pointsAmount);
                    }
                }
            }

            var groupedCharacteristics = characteristicGuidAmountPairs
                .ToDictionary(pair => new AssetReferenceT<PotionCharacteristic>(pair.Key), pair => pair.Value);
                
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