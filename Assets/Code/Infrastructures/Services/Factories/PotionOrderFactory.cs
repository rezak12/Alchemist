using System.Collections.Generic;
using System.Linq;
using Code.Infrastructures.Services.RandomServices;
using Code.Logic.Orders;
using Code.Logic.Potions;
using Code.StaticData;

namespace Code.Infrastructures.Services.Factories
{
    public class PotionOrderFactory : IPotionOrderFactory
    {
        private readonly IRandomService _randomService;

        public PotionOrderFactory(IRandomService randomService)
        {
            _randomService = randomService;
        }

        public PotionOrder CreateOrder(PotionOrderDifficulty orderDifficulty, PotionOrderType orderType)
        {
            var orderDifficultyName = orderDifficulty.Name;
            var orderTypeName = orderType.Name;
            OrderReward reward = CreateReward(orderDifficulty, orderType);
            List<PotionCharacteristicAmountPair> requirementCharacteristics = 
                CreateRequirementCharacteristics(orderDifficulty, orderType);

            return new PotionOrder(orderDifficultyName, orderTypeName, requirementCharacteristics, reward);
        }

        private List<PotionCharacteristicAmountPair> CreateRequirementCharacteristics(PotionOrderDifficulty orderDifficulty, PotionOrderType orderType)
        {
            var characteristicsAmount = orderDifficulty.RequirementCharacteristicsAmount;
            var characteristics = orderType.RequirementPotionCharacteristics.Take(characteristicsAmount);
            
            var result = new List<PotionCharacteristicAmountPair>(characteristicsAmount);
            foreach (PotionCharacteristic characteristic in characteristics)
            {
                var characteristicPointsAmount = _randomService.Next(
                    orderDifficulty.MinRequirementCharacteristicPoints,
                    orderDifficulty.MaxRequirementCharacteristicPoints);

                var characteristicAmountPair =
                    new PotionCharacteristicAmountPair(characteristic, characteristicPointsAmount);
                
                result.Add(characteristicAmountPair);
            }

            return result;
        }

        private OrderReward CreateReward(PotionOrderDifficulty orderDifficulty, PotionOrderType orderType)
        {
            var coinsAmount = _randomService
                .Next(orderDifficulty.MinCoinsAmountReward, orderDifficulty.MaxCoinsAmountReward);
            
            var reputationAmount = _randomService
                .Next(orderDifficulty.MinReputationAmountReward, orderDifficulty.MaxReputationAmountReward);
            
            IngredientData ingredient;
            if (_randomService.Next(0, 100) >= orderDifficulty.IngredientAsRewardChance)
            {
                ingredient = null;
            }
            else
            {
                ingredient = orderType
                    .PossibleRewardIngredients[_randomService.Next(0, orderType.PossibleRewardIngredients.Count)];
            }

            return new OrderReward(coinsAmount, reputationAmount, ingredient);
        }
    }
}