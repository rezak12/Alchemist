using System.Linq;
using Code.Logic.Orders;
using Code.Logic.Potions;

namespace Code.Logic.PotionMaking
{
    public class ResultPotionRater
    {
        public bool IsPotionSatisfyingRequirements(Potion potion, PotionOrder order)
        {
            var characteristicsRequirementsSatisfied = IsPotionSatisfiedCharacteristicsRequirements(order, potion);

            return characteristicsRequirementsSatisfied;
        }

        private static bool IsPotionSatisfiedCharacteristicsRequirements(PotionOrder order, Potion potion)
        {
            var requirementCharacteristicAmountPairs = order
                .RequirementCharacteristics
                .ToDictionary(pair => pair.Characteristic, pair => pair.PointsAmount);

            var createdCharacteristicAmountPairs = potion
                .CharacteristicAmountPairs
                .ToDictionary(pair => pair.Characteristic, pair => pair.PointsAmount);

            var characteristicsRequirementsSatisfied = requirementCharacteristicAmountPairs.All(requirementPair =>
            {
                if (!createdCharacteristicAmountPairs.TryGetValue(requirementPair.Key, out var amount))
                {
                    return false;
                }

                return amount >= requirementPair.Value;
            });
            
            return characteristicsRequirementsSatisfied;
        }
    }
}