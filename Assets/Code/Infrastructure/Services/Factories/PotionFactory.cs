using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Infrastructure.Services.AssetProvider;
using Code.Logic.Potions;
using Code.StaticData;

namespace Code.Infrastructure.Services.Factories
{
    public class PotionFactory : IPotionFactory
    {

        private readonly IAssetProvider _assetProvider;

        public PotionFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async Task<PotionInfo> CreatePotionAsync(IEnumerable<IngredientData> ingredients)
        {
            List<PotionCharacteristicAmountPair> characteristics = await CalculatePotionCharacteristicsAsync(ingredients);

            return new PotionInfo(characteristics);
        }

        private async Task<List<PotionCharacteristicAmountPair>> CalculatePotionCharacteristicsAsync
            (IEnumerable<IngredientData> ingredients)
        {
            var characteristicsReferences = ingredients
                .SelectMany(ingredient => ingredient.Characteristics);

            var characteristicPointsGroupedByCharacteristic = new Dictionary<PotionCharacteristic, int>();
            foreach (IngredientCharacteristicAmountPair potionCharacteristicAmountPair in characteristicsReferences)
            {
                int characteristicPointsAmount = potionCharacteristicAmountPair.PointsAmount;
                PotionCharacteristic potionCharacteristic = await _assetProvider
                    .LoadAsync<PotionCharacteristic>(potionCharacteristicAmountPair.CharacteristicReference);

                if (characteristicPointsGroupedByCharacteristic.ContainsKey(potionCharacteristic))
                {
                    characteristicPointsGroupedByCharacteristic[potionCharacteristic] += characteristicPointsAmount;
                }
                else
                {
                    characteristicPointsGroupedByCharacteristic.Add(potionCharacteristic, characteristicPointsAmount);
                }
            }

            var characteristics = characteristicPointsGroupedByCharacteristic
                .Select(pair => new PotionCharacteristicAmountPair(pair.Key, pair.Value))
                .ToList();
            return characteristics;
        }
    }
}