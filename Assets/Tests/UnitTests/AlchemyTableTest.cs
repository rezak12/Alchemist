using System.Collections.Generic;
using Code.Infrastructure.Services.Factories;
using Code.Logic.PotionMaking;
using Code.StaticData;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests.UnitTests
{
    [TestFixture]
    public class AlchemyTableTest
    {
        private const string FirstIngredientPath = "AlchemyTableTests/Ingredients/FirstIngredient";
        private const string SecondIngredientPath = "AlchemyTableTests/Ingredients/SecondIngredient";

        private IngredientData _firstIngredient;
        private IngredientData _secondIngredient;

        private AlchemyTable _unitUnderTest;

        [SetUp]
        public void Setup()
        {
            _firstIngredient = Resources.Load<IngredientData>(FirstIngredientPath);
            _secondIngredient = Resources.Load<IngredientData>(SecondIngredientPath);

            var createdSlots = CreateSlots(4);
            var potionInfoFactoryStub = Substitute.For<IPotionInfoFactory>();
            _unitUnderTest = new AlchemyTable(potionInfoFactoryStub, createdSlots);
        }

        [Test]
        public void WhenAddingIngredient_AndTableHaveFreeSlots_ThenFillOneFreeSlot()
        {
            // Arrange.

            // Act.
            _unitUnderTest.FillSlot(_firstIngredient);

            // Assert.
            Assert.That(_unitUnderTest.IsAllSlotsFree, Is.False);
        }

        [Test]
        public void WhenRemovingIngredient_ThenFreeSlot()
        {
            //Arrange

            // Act.
            _unitUnderTest.FillSlot(_firstIngredient);
            _unitUnderTest.ReleaseLastSlot();

            // Assert.
            Assert.That(_unitUnderTest.IsAllSlotsFree, Is.True);
        }

        [Test]
        public void WhenRemovingIngredient_AndAllSlotsAreFree_ThenThrowException()
        {
            // Arrange.

            // Act.
            TestDelegate attemptToRemoveIngredient = _unitUnderTest.ReleaseLastSlot;

            // Assert.
            Assert.That(attemptToRemoveIngredient, Throws.Exception);
        }

        [Test]
        public void WhenAddingIngredient_AndAllSlotsAreFilled_ThenThrowException()
        {
            // Arrange.

            // Act.
            _unitUnderTest.FillSlot(_secondIngredient);
            _unitUnderTest.FillSlot(_firstIngredient);
            _unitUnderTest.FillSlot(_secondIngredient);
            _unitUnderTest.FillSlot(_firstIngredient);

            TestDelegate attemptToAddIngredient = () => _unitUnderTest.FillSlot(_firstIngredient);

            // Assert.
            Assert.That(attemptToAddIngredient, Throws.Exception);
        }

        [Test]
        public void WhenHandlingResult_AndAnySlotAreFilled_ThenCreatePotion()
        {
            // Arrange.

            // Act.
            _unitUnderTest.FillSlot(_secondIngredient);
            TestDelegate attemptToHandleResult = async () => await _unitUnderTest.CreatePotionInfo();

            // Assert.
            Assert.That(attemptToHandleResult, Throws.Nothing);
        }

        [Test]
        public void WhenHandlingResult_AndAllSlotsAreFree_ThenCreatePotionWithoutAnyCharacteristic()
        {
            // Arrange.

            // Act.
            TestDelegate attemptToHandleResult = async () => await _unitUnderTest.CreatePotionInfo();

            // Assert.
            Assert.That(attemptToHandleResult, Throws.Nothing);
        }

        private List<AlchemyTableSlot> CreateSlots(int amount)
        {
            var createdSlots = new List<AlchemyTableSlot>(amount);
            for (int i = 0; i < amount; i++)
            {
                var go = new GameObject($"Slot {i}");
                createdSlots.Add(go.AddComponent<AlchemyTableSlot>());
            }

            return createdSlots;
        }
    }
}