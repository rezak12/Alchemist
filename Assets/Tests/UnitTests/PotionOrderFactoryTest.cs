using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.RandomServices;
using Code.Logic.Orders;
using Code.StaticData;
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
        
        private PotionOrder _createdOrder;

        [SetUp]
        public void Setup()
        {
            _difficulty = Resources.Load<PotionOrderDifficulty>(OrderDifficultyPath);
            _type = Resources.Load<PotionOrderType>(OrderTypePath);
            _characteristics = Resources.LoadAll<PotionCharacteristic>(CharacteristicsPath).Take(2).ToArray();
            
            _randomServiceMock = Substitute.For<IRandomService>();
            _assetProviderMock = Substitute.For<IAssetProvider>();
            
            _unitUnderTest = new PotionOrderFactory(_randomServiceMock, _assetProviderMock);

            _createdOrder = null;
        }

        [UnityTest]
        public IEnumerator WhenCreatingOrder_AndTypeAndDifficultyNamesNotEmpty_ThenGiveThemToOrderInstance()
        {
            // Arrange.

            // Act.
            yield return CreateOrder();

            // Assert.
            Assert.That(_createdOrder.OrderDifficultyName, Is.EqualTo(_difficulty.Name));
            Assert.That(_createdOrder.OrderTypeName, Is.EqualTo(_type.Name));
        }

        [UnityTest]
        public IEnumerator WhenCreatingOrder_AndPunishmentReputationAmountRangeIsValid_ThenGiveRandomValueWithinThisRangeToOrderInstance()
        {
            // Arrange.
            var minAmount = _difficulty.MinReputationAmountPunishment;
            var maxAmount = _difficulty.MaxReputationAmountPunishment;
            var valueInMiddleOfRange = (minAmount + maxAmount) / 2;
            
            _randomServiceMock.Next(minAmount, maxAmount).Returns(valueInMiddleOfRange);
            
            // Act.
            yield return CreateOrder();
            
            // Assert.
            Assert.That(_createdOrder.Punishment.ReputationAmount, Is.EqualTo(valueInMiddleOfRange));
        }

        [UnityTest]
        public IEnumerator WhenCreatingOrder_AndRewardCoinsAmountRangeIsValid_ThenGiveRandomValueWithinThisRangeToOrderInstance()
        {
            // Arrange.
            var minAmount = _difficulty.MinCoinsAmountReward;
            var maxAmount = _difficulty.MaxCoinsAmountReward;
            var valueInMiddleOfRange = (minAmount + maxAmount) / 2;
            
            _randomServiceMock.Next(minAmount, maxAmount).Returns(valueInMiddleOfRange);
            
            // Act.
            yield return CreateOrder();
            
            // Assert.
            Assert.That(_createdOrder.Reward.CoinsAmount, Is.EqualTo(valueInMiddleOfRange));
        }

        [UnityTest]
        public IEnumerator WhenCreatingOrder_AndRewardReputationAmountRangeIsValid_ThenGiveRandomValueWithinThisRangeToOrderInstance()
        {
            // Arrange.
            var minAmount = _difficulty.MinReputationAmountReward;
            var maxAmount = _difficulty.MaxReputationAmountReward;
            var valueInMiddleOfTheRange = (minAmount + maxAmount) / 2;
            
            _randomServiceMock.Next(minAmount, maxAmount).Returns(valueInMiddleOfTheRange);
            
            // Act.
            yield return CreateOrder();
            
            // Assert.
            Assert.That(_createdOrder.Reward.ReputationAmount, Is.EqualTo(valueInMiddleOfTheRange));
        }

        [UnityTest]
        public IEnumerator WhenCreatingOrder_AndRandomValueWhileCalculatingToPutIngredientAsRewardOrNotIsLessThanChanceToGetIngredientAsOrderReward_ThenGiveRandomIngredientFromPotionTypeDataToOrderInstance()
        {
            // Arrange.
            _randomServiceMock.Next(0, 100).Returns(_difficulty.IngredientAsRewardChance - 1);
            
            // Act.
            yield return CreateOrder();
            
            // Assert.
            Assert.That(_createdOrder.Reward.IngredientReference, Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator WhenCreatingOrder_AndRandomValueWhileCalculatingToPutIngredientAsRewardOrNotIsEqualsToChanceToGetIngredientAsOrderReward_ThenPutNullToIngredientField()
        {
            // Arrange.
            _randomServiceMock.Next(0, 100).Returns(_difficulty.IngredientAsRewardChance);
            
            // Act.
            yield return CreateOrder();
            
            // Assert.
            Assert.That(_createdOrder.Reward.IngredientReference, Is.Null);
        }

        [UnityTest]
        public IEnumerator WhenCreatingOrder_AndRandomValueWhileCalculatingToPutIngredientAsRewardOrNotGreaterThanChanceToGetIngredientAsOrderReward_ThenPutNullToIngredientField()
        {
            // Arrange.
            _randomServiceMock.Next(0, 100).Returns(_difficulty.IngredientAsRewardChance + 1);
            
            // Act.
            yield return CreateOrder();
            
            // Assert.
            Assert.That(_createdOrder.Reward.IngredientReference, Is.Null);
        }

        [UnityTest]
        public IEnumerator WhenCreatingOrder_AndRequirementCharacteristicsAmountInOrderDifficultyLessThanTotalAmountOfCharacteristicsInOrderType_ThenPutOnlyRequirementAmountToOrderInstance()
        {
            // Arrange.
            var requirementCharacteristicsAmount = _difficulty.RequirementCharacteristicsAmount;
            
            var parameter = Arg.Is<IEnumerable<AssetReference>>(i => 
                i.Count() == requirementCharacteristicsAmount);

            _assetProviderMock.LoadAsync<PotionCharacteristic>(parameter).Returns(_characteristics);

            // Act.
            yield return CreateOrder();

            // Assert.
            var possibleCharacteristicsRefs = _type
                .PossibleRequirementPotionCharacteristicsReferences;
            
            Assert.That(requirementCharacteristicsAmount, 
                Is.LessThan(possibleCharacteristicsRefs.Count));
            
            Assert.That(_createdOrder.RequirementCharacteristics.Count,
                Is.EqualTo(requirementCharacteristicsAmount));
        }

        [UnityTest]
        public IEnumerator WhenCreatingOrder_AndCharacteristicPointsAmountRangeIsValid_ThenPutRandomValuesWithinThisRangeToEachCharacteristicAmountPairs()
        {
            // Arrange.
            var parameter = Arg.Is<IEnumerable<AssetReference>>(i =>
                i.Count() == _difficulty.RequirementCharacteristicsAmount);
            
            _assetProviderMock.LoadAsync<PotionCharacteristic>(parameter).Returns(_characteristics);
            
            var minAmount = _difficulty.MinRequirementCharacteristicPointsAmount;
            var maxAmount = _difficulty.MaxRequirementCharacteristicPointsAmount;
            var valueInMiddleOfTheRange = (minAmount + maxAmount) / 2;

            _randomServiceMock.Next(minAmount, maxAmount).Returns(valueInMiddleOfTheRange);
            
            // Act.
            yield return CreateOrder();

            // Assert.
            var characteristicPointsAmountForEachPair = _createdOrder
                .RequirementCharacteristics.Select(pair => pair.PointsAmount);
            
            Assert.That(characteristicPointsAmountForEachPair, Has.All.EqualTo(valueInMiddleOfTheRange));
        }

        private IEnumerator CreateOrder()
        {
            var task = _unitUnderTest.CreateOrderAsync(_difficulty, _type);
            yield return task.AsIEnumeratorReturnNull();
            _createdOrder = task.Result;
        }
    }
}
