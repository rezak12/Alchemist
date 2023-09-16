using System.Linq;
using Code.Infrastructure.Services.Factories;
using Code.Logic.Orders;
using Code.Logic.Potions;

namespace Code.Logic.PotionMaking
{
    public class ResultPotionRater
    {
        public bool RatePotionByOrderRequirementCharacteristics(PotionOrder order, Potion potion)
        {
            var isPotionMatchesRequirementCharacteristics = IsPotionMatchesRequirementCharacteristics(order, potion);
            return isPotionMatchesRequirementCharacteristics;
        }

        private bool IsPotionMatchesRequirementCharacteristics(PotionOrder order, Potion potion)
        {
            var requirementCharacteristicAmountPairs = order
                .RequirementCharacteristics
                .ToDictionary(pair => pair.Characteristic, pair => pair.PointsAmount);
            
            var takenCharacteristicAmountPairs = potion
                .CharacteristicAmountPairs
                .ToDictionary(pair => pair.Characteristic, pair => pair.PointsAmount);

            
            foreach (var characteristicAmountPair in requirementCharacteristicAmountPairs)
            {
                if (!takenCharacteristicAmountPairs.TryGetValue(characteristicAmountPair.Key, out var amount))
                {
                    return false;
                }
                else if (amount < characteristicAmountPair.Value)
                {
                    return false;
                }
            }

            return true;
        }
    }
}