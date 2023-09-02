using System.Collections.Generic;
using System.Linq;
using Code.StaticData;

namespace Code.Logic.Potions
{
    public class PotionFactory : IPotionFactory
    {
        public PotionInfo CreatePotion(IEnumerable<IngredientData> ingredients)
        {
            List<PotionCharacteristicAmountPair> characteristics = CalculatePotionCharacteristics(ingredients);

            return new PotionInfo(characteristics);
        }

        private List<PotionCharacteristicAmountPair> CalculatePotionCharacteristics(IEnumerable<IngredientData> ingredients)
        {
            var characteristics = ingredients
                .SelectMany(ingredient => ingredient.Characteristics).ToList();

            var characteristicPointsGroupedByCharacteristic = new Dictionary<PotionCharacteristic, int>();
            foreach (PotionCharacteristicAmountPair potionCharacteristicAmountPair in characteristics)
            {
                PotionCharacteristic potionCharacteristic = potionCharacteristicAmountPair.Characteristic;
                int characteristicPointsAmount = potionCharacteristicAmountPair.PointsAmount;

                if (characteristicPointsGroupedByCharacteristic.ContainsKey(potionCharacteristic))
                {
                    characteristicPointsGroupedByCharacteristic[potionCharacteristic] += characteristicPointsAmount;
                }
                else
                {
                    characteristicPointsGroupedByCharacteristic.Add(potionCharacteristic, characteristicPointsAmount);
                }
            }

            characteristics = characteristicPointsGroupedByCharacteristic
                .Select(pair => new PotionCharacteristicAmountPair(pair.Key, pair.Value))
                .ToList();
            return characteristics;
        }
    }
}