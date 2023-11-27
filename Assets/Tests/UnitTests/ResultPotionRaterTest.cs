using System.Collections.Generic;
using Code.Logic.Orders;
using Code.Logic.PotionMaking;
using Code.Logic.Potions;
using Code.StaticData;
using NUnit.Framework;
using UnityEngine;

namespace Tests.UnitTests
{
    public class ResultPotionRaterTest
    {
        private const string FirstCharacteristicPath = "PotionResultRaterTest/Characteristics/FirstChar";
        private const string SecondCharacteristicPath = "PotionResultRaterTest/Characteristics/SecondChar";
        private const string ThirdCharacteristicPath = "PotionResultRaterTest/Characteristics/ThirdChar";

        private PotionCharacteristic _firstCharacteristic;
        private PotionCharacteristic _secondCharacteristic;
        private PotionCharacteristic _thirdCharacteristic;
        
        private ResultPotionRater _unitUnderTest;

        [SetUp]
        public void Setup()
        {
            _firstCharacteristic = Resources.Load<PotionCharacteristic>(FirstCharacteristicPath);
            _secondCharacteristic = Resources.Load<PotionCharacteristic>(SecondCharacteristicPath);
            _thirdCharacteristic = Resources.Load<PotionCharacteristic>(ThirdCharacteristicPath);

            _unitUnderTest = new ResultPotionRater();
        }

        [Test]
        public void WhenRatingPotion_AndPotionHaveAllRequirementCharacteristicsWithPointsAmountEqualsOrGreaterThanRequirementForEach_ThenReturnTrue()
        {
            // Arrange.
            var requirementCharacteristicAmountPairs = new List<PotionCharacteristicAmountPair>()
            {
                new (_firstCharacteristic, 10),
                new (_secondCharacteristic, 10)
            };
            var createdCharacteristicAmountPairs = new List<PotionCharacteristicAmountPair>()
            {
                new (_firstCharacteristic, 10),
                new (_secondCharacteristic, 15),
                new (_thirdCharacteristic, 5)
            };

            PotionOrder order = CreateOrder(requirementCharacteristicAmountPairs);
            Potion potion = CreatePotion(createdCharacteristicAmountPairs);
            
            // Act.
            var potionSatisfyingRequirements = _unitUnderTest.IsPotionSatisfyingRequirements(potion, order);
            
            // Assert.
            Assert.That(potionSatisfyingRequirements, Is.True);
        }

        [Test]
        public void WhenRatingPotion_AndPotionHaveAnyRequirementCharacteristicWithPointsAmountLessThanRequirement_ThenReturnFalse()
        {
            // Arrange.
            var requirementCharacteristicAmountPairs = new List<PotionCharacteristicAmountPair>()
            {
                new (_firstCharacteristic, 10),
                new (_secondCharacteristic, 10)
            };
            var createdCharacteristicAmountPairs = new List<PotionCharacteristicAmountPair>()
            {
                new (_firstCharacteristic, 10),
                new (_secondCharacteristic, 8)
            };

            PotionOrder order = CreateOrder(requirementCharacteristicAmountPairs);
            Potion potion = CreatePotion(createdCharacteristicAmountPairs);
            
            // Act.
            var potionSatisfyingRequirements = _unitUnderTest.IsPotionSatisfyingRequirements(potion, order);
            
            // Assert.
            Assert.That(potionSatisfyingRequirements, Is.False);
        }

        [Test]
        public void WhenRatingPotion_AndPotionHaveNotAnyRequirementCharacteristic_ThenReturnFalse()
        {
            // Arrange.
            var requirementCharacteristicAmountPairs = new List<PotionCharacteristicAmountPair>()
            {
                new (_firstCharacteristic, 10),
                new (_secondCharacteristic, 10)
            };
            var createdCharacteristicAmountPairs = new List<PotionCharacteristicAmountPair>()
            {
                new (_firstCharacteristic, 10)
            };

            PotionOrder order = CreateOrder(requirementCharacteristicAmountPairs);
            Potion potion = CreatePotion(createdCharacteristicAmountPairs);
            
            // Act.
            var potionSatisfyingRequirements = _unitUnderTest.IsPotionSatisfyingRequirements(potion, order);
            
            // Assert.
            Assert.That(potionSatisfyingRequirements, Is.False);
        }

        private Potion CreatePotion(IEnumerable<PotionCharacteristicAmountPair> createdCharacteristicAmountPairs)
        {
            var potion = new GameObject("potion").AddComponent<Potion>();
            potion.Initialize(new PotionInfo(createdCharacteristicAmountPairs), null);
            return potion;
        }

        private PotionOrder CreateOrder(IEnumerable<PotionCharacteristicAmountPair> requirementCharacteristicAmountPairs)
        {
            return new PotionOrder(null, null, requirementCharacteristicAmountPairs, null, null);
        }
    }
}