using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.RandomServices;
using Code.Logic.Orders;
using Code.StaticData;
using Code.StaticData.Orders;
using Code.StaticData.Potions;
using Cysharp.Threading.Tasks;
using NSubstitute;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

namespace Tests.UnitTests
{
    [TestFixture]
    public class PotionOrderFactoryTest
    {
        private const string OrderDifficultyPath = "PotionOrderFactoryTest/TestOrderDifficulty";
        private const string OrderTypePath = "PotionOrderFactoryTest/TestOrderType";
        private const string CharacteristicsPath = "PotionOrderFactoryTest/Characteristics";

        private PotionOrderDifficulty _difficulty;
        private PotionOrderType _type;
        private PotionCharacteristic[] _characteristics;
        
        private PotionOrderFactory _unitUnderTest;
        private IRandomService _randomServiceMock;
        private IAssetProvider _assetProviderMock;

        [SetUp]
        public void Setup()
        {
            _difficulty = Resources.Load<PotionOrderDifficulty>(OrderDifficultyPath);
            _type = Resources.Load<PotionOrderType>(OrderTypePath);
            _characteristics = Resources.LoadAll<PotionCharacteristic>(CharacteristicsPath).Take(2).ToArray();
            
            _randomServiceMock = Substitute.For<IRandomService>();
            _assetProviderMock = Substitute.For<IAssetProvider>();
            
            _unitUnderTest = new PotionOrderFactory(_randomServiceMock, _assetProviderMock);
        }
        
        [UnityTest]
        public IEnumerator WhenCreatingOrder_AndTypeAndDifficultyNamesNotEmpty_ThenGiveThemToOrderInstance() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                MockCommonAssetProviderLoadResult();
                
                // Act.
                PotionOrder createdOrder = await _unitUnderTest.CreateOrderAsync(_difficulty, _type);

                // Assert.
                Assert.That(createdOrder.OrderDifficultyName, Is.EqualTo(_difficulty.Name));
                Assert.That(createdOrder.OrderTypeName, Is.EqualTo(_type.Name));
            });

        [UnityTest]
        public IEnumerator
            WhenCreatingOrder_AndPunishmentReputationAmountRangeIsValid_ThenGiveRandomValueWithinThisRangeToOrderInstance() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                var minAmount = _difficulty.MinReputationAmountPunishment;
                var maxAmount = _difficulty.MaxReputationAmountPunishment;
                var valueInMiddleOfRange = (minAmount + maxAmount) / 2;

                _randomServiceMock.Next(minAmount, maxAmount).Returns(valueInMiddleOfRange);
                
                MockCommonAssetProviderLoadResult();

                // Act.
                PotionOrder createdOrder = await _unitUnderTest.CreateOrderAsync(_difficulty, _type);

                // Assert.
                Assert.That(createdOrder.Punishment.ReputationAmount, Is.EqualTo(valueInMiddleOfRange));
            });

        [UnityTest]
        public IEnumerator
            WhenCreatingOrder_AndRewardCoinsAmountRangeIsValid_ThenGiveRandomValueWithinThisRangeToOrderInstance() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                var minAmount = _difficulty.MinCoinsAmountReward;
                var maxAmount = _difficulty.MaxCoinsAmountReward;
                var valueInMiddleOfRange = (minAmount + maxAmount) / 2;

                _randomServiceMock.Next(minAmount, maxAmount).Returns(valueInMiddleOfRange);
                
                MockCommonAssetProviderLoadResult();

                // Act.
                PotionOrder createdOrder = await _unitUnderTest.CreateOrderAsync(_difficulty, _type);

                // Assert.
                Assert.That(createdOrder.Reward.CoinsAmount, Is.EqualTo(valueInMiddleOfRange));
            });

        [UnityTest]
        public IEnumerator
            WhenCreatingOrder_AndRewardReputationAmountRangeIsValid_ThenGiveRandomValueWithinThisRangeToOrderInstance() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                var minAmount = _difficulty.MinReputationAmountReward;
                var maxAmount = _difficulty.MaxReputationAmountReward;
                var valueInMiddleOfTheRange = (minAmount + maxAmount) / 2;

                _randomServiceMock.Next(minAmount, maxAmount).Returns(valueInMiddleOfTheRange);
                
                MockCommonAssetProviderLoadResult();

                // Act.
                PotionOrder createdOrder = await _unitUnderTest.CreateOrderAsync(_difficulty, _type);

                // Assert.
                Assert.That(createdOrder.Reward.ReputationAmount, Is.EqualTo(valueInMiddleOfTheRange));
            });

        [UnityTest]
        public IEnumerator
            WhenCreatingOrder_AndRandomValueWhileCalculatingToPutIngredientAsRewardOrNotIsLessThanChanceToGetIngredientAsOrderReward_ThenGiveRandomIngredientFromPotionTypeDataToOrderInstance() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                _randomServiceMock.Next(0, 100).Returns(_difficulty.IngredientAsRewardChance - 1);

                MockCommonAssetProviderLoadResult();
                
                // Act.
                PotionOrder createdOrder = await _unitUnderTest.CreateOrderAsync(_difficulty, _type);

                // Assert.
                Assert.That(createdOrder.Reward.IngredientReference, Is.Not.Null);
            });

        [UnityTest]
        public IEnumerator
            WhenCreatingOrder_AndRandomValueWhileCalculatingToPutIngredientAsRewardOrNotIsEqualsToChanceToGetIngredientAsOrderReward_ThenPutNullToIngredientField() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                _randomServiceMock.Next(0, 100).Returns(_difficulty.IngredientAsRewardChance);
                
                MockCommonAssetProviderLoadResult();

                // Act.
                PotionOrder createdOrder = await _unitUnderTest.CreateOrderAsync(_difficulty, _type);

                // Assert.
                Assert.That(createdOrder.Reward.IngredientReference, Is.Null);
            });

        [UnityTest]
        public IEnumerator
            WhenCreatingOrder_AndRandomValueWhileCalculatingToPutIngredientAsRewardOrNotGreaterThanChanceToGetIngredientAsOrderReward_ThenPutNullToIngredientField() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                _randomServiceMock.Next(0, 100).Returns(_difficulty.IngredientAsRewardChance + 1);
                
                MockCommonAssetProviderLoadResult();

                // Act.
                PotionOrder createdOrder = await _unitUnderTest.CreateOrderAsync(_difficulty, _type);

                // Assert.
                Assert.That(createdOrder.Reward.IngredientReference, Is.Null);
            });

        [UnityTest]
        public IEnumerator
            WhenCreatingOrder_AndRequirementCharacteristicsAmountInOrderDifficultyLessThanTotalAmountOfCharacteristicsInOrderType_ThenPutOnlyRequirementAmountToOrderInstance() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                var requirementCharacteristicsAmount = _difficulty.RequirementCharacteristicsAmount;

                var parameter = Arg.Is<IEnumerable<AssetReference>>(i =>
                    i.Count() == requirementCharacteristicsAmount);

                _assetProviderMock.LoadAsync<PotionCharacteristic>(parameter)
                    .Returns(new UniTask<PotionCharacteristic[]>(_characteristics));

                // Act.
                PotionOrder createdOrder = await _unitUnderTest.CreateOrderAsync(_difficulty, _type);

                // Assert.
                var possibleCharacteristicsRefs = _type
                    .PossibleRequirementPotionCharacteristicsReferences;

                Assert.That(requirementCharacteristicsAmount,
                    Is.LessThan(possibleCharacteristicsRefs.Count));

                Assert.That(createdOrder.RequirementCharacteristics.Count,
                    Is.EqualTo(requirementCharacteristicsAmount));
            });

        [UnityTest]
        public IEnumerator
            WhenCreatingOrder_AndCharacteristicPointsAmountRangeIsValid_ThenPutRandomValuesWithinThisRangeToEachCharacteristicAmountPairs() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                var parameter = Arg.Is<IEnumerable<AssetReference>>(i =>
                    i.Count() == _difficulty.RequirementCharacteristicsAmount);

                _assetProviderMock.LoadAsync<PotionCharacteristic>(parameter)
                    .Returns(new UniTask<PotionCharacteristic[]>(_characteristics));

                var minAmount = _difficulty.MinRequirementCharacteristicPointsAmount;
                var maxAmount = _difficulty.MaxRequirementCharacteristicPointsAmount;
                var valueInMiddleOfTheRange = (minAmount + maxAmount) / 2;

                _randomServiceMock.Next(minAmount, maxAmount).Returns(valueInMiddleOfTheRange);

                // Act.
                PotionOrder createdOrder = await _unitUnderTest.CreateOrderAsync(_difficulty, _type);

                // Assert.
                var characteristicPointsAmountForEachPair = createdOrder
                    .RequirementCharacteristics.Select(pair => pair.PointsAmount);

                Assert.That(characteristicPointsAmountForEachPair, Has.All.EqualTo(valueInMiddleOfTheRange));
            });

        private void MockCommonAssetProviderLoadResult()
        {
            _assetProviderMock.LoadAsync<PotionCharacteristic>(Arg.Any<IEnumerable<AssetReference>>())
                .Returns(new UniTask<PotionCharacteristic[]>(_characteristics));
        }
    }
}
