using System.Collections.Generic;
using System.Linq;
using Code.Data;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.RandomServices;
using Code.Logic.Orders;
using Code.Logic.Potions;
using Code.StaticData;
using Code.StaticData.Ingredients;
using Code.StaticData.Orders;
using Code.StaticData.Potions;
using Cysharp.Threading.Tasks;
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

        public async UniTask<PotionOrder> CreateOrderAsync(PotionOrderDifficulty orderDifficulty, PotionOrderType orderType)
        {
            string orderDifficultyName = orderDifficulty.Name;
            string orderTypeName = orderType.Name;
            
            List<PotionCharacteristicAmountPair> requirementCharacteristics = 
                await CreateRequirementCharacteristicsAsync(orderDifficulty, orderType);
            
            PotionOrderReward reward = CreateReward(orderDifficulty, orderType);
            PotionOrderPunishment punishment = CreatePunishment(orderDifficulty);

            return new PotionOrder(orderDifficultyName, orderTypeName, requirementCharacteristics, reward, punishment);
        }

        private async UniTask<List<PotionCharacteristicAmountPair>> CreateRequirementCharacteristicsAsync
            (PotionOrderDifficulty orderDifficulty, PotionOrderType orderType)
        {
            int characteristicsAmount = orderDifficulty.RequirementCharacteristicsAmount;
            
            IEnumerable<AssetReferenceT<PotionCharacteristic>> characteristicsReferences = orderType
                .PossibleRequirementPotionCharacteristicsReferences
                .Shuffle()
                .Take(characteristicsAmount);
            
            PotionCharacteristic[] characteristics = await _assetProvider
                .LoadAsync<PotionCharacteristic>(characteristicsReferences);

            var result = new List<PotionCharacteristicAmountPair>(characteristicsAmount);
            foreach (PotionCharacteristic characteristic in characteristics)
            {
                int characteristicPointsAmount = _randomService.Next(
                    orderDifficulty.MinRequirementCharacteristicPointsAmount,
                    orderDifficulty.MaxRequirementCharacteristicPointsAmount);

                var characteristicAmountPair =
                    new PotionCharacteristicAmountPair(characteristic, characteristicPointsAmount);
                
                result.Add(characteristicAmountPair);
            }

            return result;
        }

        private PotionOrderReward CreateReward(PotionOrderDifficulty orderDifficulty, PotionOrderType orderType)
        {
            int coinsAmount = _randomService
                .Next(orderDifficulty.MinCoinsAmountReward, orderDifficulty.MaxCoinsAmountReward);
            
            int reputationAmount = _randomService
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
            int reputationAmount = _randomService
                .Next(orderDifficulty.MinReputationAmountPunishment, orderDifficulty.MaxReputationAmountPunishment);

            return new PotionOrderPunishment(reputationAmount);
        }
    }
}