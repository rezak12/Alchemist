using System.Collections.Generic;
using Code.Logic.Potions;

namespace Code.Logic.Orders
{
    public class PotionOrder
    {
        public string OrderDifficultyName { get; }
        public string OrderTypeName { get; }
        public List<PotionCharacteristicAmountPair> RequirementCharacteristics { get; }
        public OrderReward Reward { get; }

        public PotionOrder(string orderDifficultyName, string orderTypeName, List<PotionCharacteristicAmountPair> requirementCharacteristics, OrderReward reward)
        {
            OrderDifficultyName = orderDifficultyName;
            OrderTypeName = orderTypeName;
            RequirementCharacteristics = requirementCharacteristics;
            Reward = reward;
        }
    }
}