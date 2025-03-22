using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Infrastructure.Services.Factories;
using Code.Logic.PotionMaking;
using Code.Logic.Potions;
using Code.StaticData.Ingredients;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UnitTests
{
    [TestFixture]
    public class AlchemyTableTest
    {
        private const string FirstIngredientPath = "AlchemyTableTests/Ingredients/FirstIngredient";
        private const string SecondIngredientPath = "AlchemyTableTests/Ingredients/SecondIngredient";

        private IngredientData _firstIngredient;
        private IngredientData _secondIngredient;

        private AlchemyTable _tableUnderTest;
        private IPotionInfoFactory _potionInfoFactoryStub;

        [SetUp]
        public void Setup()
        {
            _firstIngredient = Resources.Load<IngredientData>(FirstIngredientPath);
            _secondIngredient = Resources.Load<IngredientData>(SecondIngredientPath);

            List<AlchemyTableSlot> createdSlots = CreateSlots(4);
            _potionInfoFactoryStub = Substitute.For<IPotionInfoFactory>();
            _tableUnderTest = new AlchemyTable(_potionInfoFactoryStub, createdSlots);
        }

        [Test]
        public void WhenAddingIngredient_AndTableHasFreeSlots_ThenFillOneFreeSlot()
        {
            // Arrange.

            // Act.
            _tableUnderTest.FillSlot(_firstIngredient);

            // Assert.
            bool areAllSlotsFree = _tableUnderTest.IsAllSlotsFree;
            areAllSlotsFree
                .Should()
                .BeFalse("an ingredient was added");
        }

        [Test]
        public void WhenRemovingIngredient_ThenFreeSlot()
        {
            //Arrange

            // Act.
            _tableUnderTest.FillSlot(_firstIngredient);
            _tableUnderTest.ReleaseLastSlot();

            // Assert.
            bool areAllSlotsFree = _tableUnderTest.IsAllSlotsFree;
            areAllSlotsFree
                .Should()
                .BeTrue("only one element was added, after it's removal all slots should be free");
        }

        [Test]
        public void WhenRemovingIngredient_AndAllSlotsAreFree_ThenThrowException()
        {
            // Arrange.

            // Act.
            Action attemptToRemoveIngredient = _tableUnderTest.ReleaseLastSlot;

            // Assert.
            attemptToRemoveIngredient
                .Should()
                .Throw<InvalidOperationException>("all slots are free and the stack for filled slots is empty");
        }

        [Test]
        public void WhenAddingIngredient_AndAllSlotsAreFilled_ThenThrowException()
        {
            // Arrange.

            // Act.
            _tableUnderTest.FillSlot(_secondIngredient);
            _tableUnderTest.FillSlot(_firstIngredient);
            _tableUnderTest.FillSlot(_secondIngredient);
            _tableUnderTest.FillSlot(_firstIngredient);

            Action attemptToAddIngredient = () => _tableUnderTest.FillSlot(_firstIngredient);

            // Assert.
            attemptToAddIngredient
                .Should()
                .Throw<InvalidOperationException>("all slots are filled and the stack for them is empty");
        }

        [UnityTest]
        public IEnumerator WhenHandlingResult_AndAnySlotIsFilled_ThenCreatePotion() => 
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.

                // Act.
                _tableUnderTest.FillSlot(_secondIngredient);
                Func<Task<PotionInfo>> attemptToCreate = async () => await _tableUnderTest.CreatePotionInfo();

                // Assert.
                await attemptToCreate.Should().NotThrowAsync();
                
                var expectedIngredientsList = Arg.Is<List<IngredientData>>(list => list.Count == 1 && list.Contains(_secondIngredient));
                _potionInfoFactoryStub.Received().CreatePotionInfoAsync(expectedIngredientsList);
            });

        [UnityTest]
        public IEnumerator WhenHandlingResult_AndAllSlotsAreFree_ThenCreatePotionWithoutAnyCharacteristic() =>
            UniTask.ToCoroutine(async () => 
            {
                // Arrange.

                // Act.
                Func<Task<PotionInfo>> attemptToCreate = async () => await _tableUnderTest.CreatePotionInfo();
                    
                // Assert.
                await attemptToCreate.Should().NotThrowAsync();
                    
                var expectedIngredientsList = Arg.Is<List<IngredientData>>(list => list.Count == 0);
                _potionInfoFactoryStub.Received().CreatePotionInfoAsync(expectedIngredientsList);
            });

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