using System.Collections.Generic;
using System.Linq;
using Code.Logic.Orders;
using Code.Logic.Potions;
using Code.StaticData;

namespace Code.Logic.PotionMaking
{
    public class ResultPotionRater
    {
        public bool IsPotionSatisfyingRequirements(Potion potion, PotionOrder order) => 
            IsPotionSatisfyingCharacteristicsRequirements(order, potion);

        private static bool IsPotionSatisfyingCharacteristicsRequirements(PotionOrder order, Potion potion)
        {
            Dictionary<PotionCharacteristic, int> requirementCharacteristicAmountPairs = order
                .RequirementCharacteristics
                .ToDictionary(pair => pair.Characteristic, pair => pair.PointsAmount);

            Dictionary<PotionCharacteristic, int> createdCharacteristicAmountPairs = potion
                .CharacteristicAmountPairs
                .ToDictionary(pair => pair.Characteristic, pair => pair.PointsAmount);

            bool characteristicsRequirementsSatisfied = requirementCharacteristicAmountPairs.All(requirementPair =>
            {
                if (!createdCharacteristicAmountPairs.TryGetValue(requirementPair.Key, out int amount))
                {
                    return false;
                }

                return amount >= requirementPair.Value;
            });
            
            return characteristicsRequirementsSatisfied;
        }
    }
}