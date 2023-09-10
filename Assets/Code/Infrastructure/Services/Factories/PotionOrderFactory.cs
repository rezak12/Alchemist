using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.RandomServices;
using Code.Logic.Orders;
using Code.Logic.Potions;
using Code.StaticData;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.Factories
{
    public class PotionOrderFactory : IPotionOrderFactory
    {
        private readonly IRandomService _randomService;
        private readonly IAssetProvider _assetProvider;

        public PotionOrderFactory(IRandomService randomService, IAssetProvider assetProvider)
        {
            _randomService = randomService;
            _assetProvider = assetProvider;
        }

        public async Task<PotionOrder> CreateOrderAsync(PotionOrderDifficulty orderDifficulty, PotionOrderType orderType)
        {
            var orderDifficultyName = orderDifficulty.Name;
            var orderTypeName = orderType.Name;
            
            List<PotionCharacteristicAmountPair> requirementCharacteristics = 
                await CreateRequirementCharacteristicsAsync(orderDifficulty, orderType);
            
            PotionOrderReward reward = await CreateRewardAsync(orderDifficulty, orderType);

            return new PotionOrder(orderDifficultyName, orderTypeName, requirementCharacteristics, reward);
        }

        private async Task<List<PotionCharacteristicAmountPair>> CreateRequirementCharacteristicsAsync
            (PotionOrderDifficulty orderDifficulty, PotionOrderType orderType)
        {
            var characteristicsAmount = orderDifficulty.RequirementCharacteristicsAmount;
            
            var characteristicsReferences = orderType
                .RequirementPotionCharacteristicsReferences
                .Take(characteristicsAmount)
                .ToList();
            
            var characteristics = await _assetProvider
                .LoadAsync<PotionCharacteristic>(characteristicsReferences);

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

        private async Task<PotionOrderReward> CreateRewardAsync(PotionOrderDifficulty orderDifficulty, PotionOrderType orderType)
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
                var reference = orderType
                    .PossibleRewardIngredientsReferences[
                        _randomService.Next(0, orderType.PossibleRewardIngredientsReferences.Count)];
                ingredient = await _assetProvider.LoadAsync<IngredientData>(reference);
            }

            return new PotionOrderReward(coinsAmount, reputationAmount, ingredient);
        }
    }
}