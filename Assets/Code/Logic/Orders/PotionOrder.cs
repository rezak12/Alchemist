using System.Collections.Generic;
using Code.Logic.Potions;

namespace Code.Logic.Orders
{
    public class PotionOrder
    {
        public string OrderDifficultyName { get; }
        public string OrderTypeName { get; }
        public IReadOnlyCollection<PotionCharacteristicAmountPair> RequirementCharacteristics { get; }
        public PotionOrderReward Reward { get; }
        public PotionOrderPunishment Punishment { get; }

        public PotionOrder(
            string orderDifficultyName, 
            string orderTypeName, 
            IReadOnlyCollection<PotionCharacteristicAmountPair> requirementCharacteristics, 
            PotionOrderReward reward, 
            PotionOrderPunishment punishment)
        {
            OrderDifficultyName = orderDifficultyName;
            OrderTypeName = orderTypeName;
            RequirementCharacteristics = requirementCharacteristics;
            Reward = reward;
            Punishment = punishment;
        }
    }
}