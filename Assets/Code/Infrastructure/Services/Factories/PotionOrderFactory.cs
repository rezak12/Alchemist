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
            
            PotionOrderReward reward = CreateReward(orderDifficulty, orderType);
            PotionOrderPunishment punishment = CreatePunishment(orderDifficulty);

            return new PotionOrder(orderDifficultyName, orderTypeName, requirementCharacteristics, reward, punishment);
        }

        private async Task<List<PotionCharacteristicAmountPair>> CreateRequirementCharacteristicsAsync
            (PotionOrderDifficulty orderDifficulty, PotionOrderType orderType)
        {
            var characteristicsAmount = orderDifficulty.RequirementCharacteristicsAmount;
            
            var characteristicsReferences = orderType
                .PossibleRequirementPotionCharacteristicsReferences
                .OrderBy( reference => _randomService.Next(0, 100))
                .Take(characteristicsAmount);
            
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

        private PotionOrderReward CreateReward(PotionOrderDifficulty orderDifficulty, PotionOrderType orderType)
        {
            var coinsAmount = _randomService
                .Next(orderDifficulty.MinCoinsAmountReward, orderDifficulty.MaxCoinsAmountReward);
            
            var reputationAmount = _randomService
                .Next(orderDifficulty.MinReputationAmountReward, orderDifficulty.MaxReputationAmountReward);
            
            AssetReferenceT<IngredientData> ingredientReference;
            if (_randomService.Next(0, 100) < orderDifficulty.IngredientAsRewardChance)
            {
                ingredientReference = orderType
                    .PossibleRewardIngredientsReferences[_randomService
                        .Next(0, orderType.PossibleRewardIngredientsReferences.Count)];
            }
            else
            {
                ingredientReference = null;
            }

            return new PotionOrderReward(coinsAmount, reputationAmount, ingredientReference);
        }

        private PotionOrderPunishment CreatePunishment(PotionOrderDifficulty orderDifficulty)
        {
            var reputationAmount = _randomService
                .Next(orderDifficulty.MinReputationAmountPunishment, orderDifficulty.MaxReputationAmountPunishment);

            return new PotionOrderPunishment(reputationAmount);
        }
    }
}