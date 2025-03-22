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
using FluentAssertions;
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
                createdOrder.OrderDifficultyName.Should().Be(_difficulty.Name);
                createdOrder.OrderTypeName.Should().Be(_type.Name);
            });

        [UnityTest]
        public IEnumerator WhenCreatingOrder_AndPunishmentReputationAmountRangeIsValid_ThenGiveRandomValueWithinRangeToOrder() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                int minAmount = _difficulty.MinReputationAmountPunishment;
                int maxAmount = _difficulty.MaxReputationAmountPunishment;
                int mockedRandomValue = (minAmount + maxAmount) / 2;

                _randomServiceMock.Next(minAmount, maxAmount).Returns(mockedRandomValue);
                
                MockCommonAssetProviderLoadResult();

                // Act.
                PotionOrder createdOrder = await _unitUnderTest.CreateOrderAsync(_difficulty, _type);

                // Assert.
                createdOrder.Punishment.ReputationAmount.Should().Be(mockedRandomValue);
            });

        [UnityTest]
        public IEnumerator WhenCreatingOrder_AndRewardCoinsAmountRangeIsValid_ThenGiveRandomValueWithinRangeToOrder() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                int minAmount = _difficulty.MinCoinsAmountReward;
                int maxAmount = _difficulty.MaxCoinsAmountReward;
                int mockedRandomValue = (minAmount + maxAmount) / 2;

                _randomServiceMock.Next(minAmount, maxAmount).Returns(mockedRandomValue);
                
                MockCommonAssetProviderLoadResult();

                // Act.
                PotionOrder createdOrder = await _unitUnderTest.CreateOrderAsync(_difficulty, _type);

                // Assert.
                createdOrder.Reward.CoinsAmount.Should().Be(mockedRandomValue);
            });

        [UnityTest]
        public IEnumerator WhenCreatingOrder_AndRewardReputationAmountRangeIsValid_ThenGiveRandomValueWithinRangeToOrder() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                int minAmount = _difficulty.MinReputationAmountReward;
                int maxAmount = _difficulty.MaxReputationAmountReward;
                int mockedRandomValue = (minAmount + maxAmount) / 2;

                _randomServiceMock.Next(minAmount, maxAmount).Returns(mockedRandomValue);
                
                MockCommonAssetProviderLoadResult();

                // Act.
                PotionOrder createdOrder = await _unitUnderTest.CreateOrderAsync(_difficulty, _type);

                // Assert.
                createdOrder.Reward.ReputationAmount.Should().Be(mockedRandomValue);
            });

        [UnityTest]
        public IEnumerator WhenCreatingOrder_AndRandomChanceValueToPutIngredientAsRewardIsLessThanOverallChance_ThenGiveRandomIngredientForCurrentTypeToOrder() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                _randomServiceMock.Next(0, 100).Returns(_difficulty.IngredientAsRewardChance - 1);

                MockCommonAssetProviderLoadResult();
                
                // Act.
                PotionOrder createdOrder = await _unitUnderTest.CreateOrderAsync(_difficulty, _type);

                // Assert.
                createdOrder.Reward.IngredientReference.Should().NotBeNull();
            });

        [UnityTest]
        public IEnumerator WhenCreatingOrder_AndRandomChanceValueToPutIngredientAsRewardIsGreaterOrEqualsToOverallChance_ThenPutNull() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                _randomServiceMock.Next(0, 100).Returns(_difficulty.IngredientAsRewardChance);
                
                MockCommonAssetProviderLoadResult();

                // Act.
                PotionOrder createdOrder = await _unitUnderTest.CreateOrderAsync(_difficulty, _type);

                // Assert.
                createdOrder.Reward.IngredientReference.Should().BeNull();
            });

        [UnityTest]
        public IEnumerator WhenCreatingOrder_AndRequiredCharacteristicsAmountForCurrentDifficultyIsLessThanAmountAvailableForCurrentType_ThenPutRequirementAmountToOrder() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                int requirementCharacteristicsAmount = _difficulty.RequirementCharacteristicsAmount;

                var parameter = Arg.Is<IEnumerable<AssetReference>>(i =>
                    i.Count() == requirementCharacteristicsAmount);

                _assetProviderMock.LoadAsync<PotionCharacteristic>(parameter)
                    .Returns(new UniTask<PotionCharacteristic[]>(_characteristics));

                // Act.
                PotionOrder createdOrder = await _unitUnderTest.CreateOrderAsync(_difficulty, _type);

                // Assert.
                var possibleCharacteristicsRefs = _type
                    .PossibleRequirementPotionCharacteristicsReferences;

                requirementCharacteristicsAmount.Should().BeLessThan(possibleCharacteristicsRefs.Count);
                createdOrder.RequirementCharacteristics.Count.Should().Be(requirementCharacteristicsAmount);
            });

        [UnityTest]
        public IEnumerator WhenCreatingOrder_AndCharacteristicPointsAmountRangeIsValid_ThenPutRandomValuesWithinRangeToEachPair() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                var parameter = Arg.Is<IEnumerable<AssetReference>>(i =>
                    i.Count() == _difficulty.RequirementCharacteristicsAmount);

                _assetProviderMock.LoadAsync<PotionCharacteristic>(parameter)
                    .Returns(new UniTask<PotionCharacteristic[]>(_characteristics));

                int minAmount = _difficulty.MinRequirementCharacteristicPointsAmount;
                int maxAmount = _difficulty.MaxRequirementCharacteristicPointsAmount;
                int mockedRandomValue = (minAmount + maxAmount) / 2;

                _randomServiceMock.Next(minAmount, maxAmount).Returns(mockedRandomValue);

                // Act.
                PotionOrder createdOrder = await _unitUnderTest.CreateOrderAsync(_difficulty, _type);

                // Assert.
                IEnumerable<int> pointsAmountForEachPair = createdOrder
                    .RequirementCharacteristics.Select(pair => pair.PointsAmount);

                Assert.That(pointsAmountForEachPair, Has.All.EqualTo(mockedRandomValue));
                //pointsAmountForEachPair.Should().AllSatisfy(amount => amount.Should().Be(mockedRandomValue));
            });

        private void MockCommonAssetProviderLoadResult()
        {
            _assetProviderMock.LoadAsync<PotionCharacteristic>(Arg.Any<IEnumerable<AssetReference>>())
                .Returns(new UniTask<PotionCharacteristic[]>(_characteristics));
        }
    }
}
