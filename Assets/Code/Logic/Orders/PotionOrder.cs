using System.Collections.Generic;
using Code.Logic.Potions;

namespace Code.Logic.Orders
{
    public class PotionOrder
    {
        public string OrderDifficultyName { get; }
        public string OrderTypeName { get; }
        public List<PotionCharacteristicAmountPair> RequirementCharacteristics { get; }
        public PotionOrderReward Reward { get; }

        public PotionOrder(string orderDifficultyName, string orderTypeName, List<PotionCharacteristicAmountPair> requirementCharacteristics, PotionOrderReward reward)
        {
            OrderDifficultyName = orderDifficultyName;
            OrderTypeName = orderTypeName;
            RequirementCharacteristics = requirementCharacteristics;
            Reward = reward;
        }
    }
}