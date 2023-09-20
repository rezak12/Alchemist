using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.Factories;
using Code.Logic.Potions;
using Code.StaticData;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UnitTests
{
    public class PotionInfoFactoryTest
    {
        private const string IngredientWithOneCharacteristicPath = "PotionInfoFactoryTest/SecondIngr";
        private const string IngredientWithTwoCharacteristicsPath = "PotionInfoFactoryTest/FirstIngr";
        private const string SecondIngredientWithTwoCharacteristicsPath = "PotionInfoFactoryTest/ThirdIngr";
        
        private IngredientData _ingredientWithOneCharacteristic;
        private IngredientData _ingredientWithTwoCharacteristics;
        private IngredientData _secondIngredientWithTwoCharacteristics;
        
        private PotionInfoFactory _unitUnderTest;
        
        private PotionInfo _createdPotion;

        [SetUp]
        public void Setup()
        {
            _ingredientWithTwoCharacteristics = Resources
                .Load<IngredientData>(IngredientWithTwoCharacteristicsPath);
            _ingredientWithOneCharacteristic = Resources
                .Load<IngredientData>(IngredientWithOneCharacteristicPath);
            _secondIngredientWithTwoCharacteristics = Resources
                .Load<IngredientData>(SecondIngredientWithTwoCharacteristicsPath);
            
            var assetProviderStub = Substitute.For<IAssetProvider>();
            _unitUnderTest = new PotionInfoFactory(assetProviderStub);

            _createdPotion = null;
        }

        [UnityTest]
        public IEnumerator WhenCreatingPotion_AndPuttedOneIngredientWithOneCharacteristic_ThenPutOnlyThisCharacteristicToPotionInstance()
        {
            // Arrange.
            IngredientData ingredient = _ingredientWithOneCharacteristic;
            var ingredients = new List<IngredientData>() { ingredient };

            // Act.
            yield return CreatePotionInfo(ingredients);

            // Assert.
            var characteristicsAmountPairs = _createdPotion
                .CharacteristicsAmountPairs.ToHashSet();
            
            Assert.That(characteristicsAmountPairs, Has.Count.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator WhenCreatingPotion_AndPuttedMoreThanOneIngredientsWithUniqueCharacteristics_ThenPutThemAllToPotionInstance()
        {
            // Arrange.
            IngredientData firstIngredient = _ingredientWithOneCharacteristic;
            IngredientData secondIngredient = _secondIngredientWithTwoCharacteristics;
            var ingredients = new List<IngredientData>() { firstIngredient, secondIngredient };

            // Act.
            yield return CreatePotionInfo(ingredients);

            // Assert.
            var potionCharacteristics = _createdPotion
                .CharacteristicsAmountPairs
                .ToList();

            var puttedUniqueCharacteristicsAmount = 3;

            Assert.That(potionCharacteristics, Has.Count.EqualTo(puttedUniqueCharacteristicsAmount));
        }

        [UnityTest]
        public IEnumerator WhenCreatingPotion_AndPuttedIngredientsWithSomeEqualCharacteristics_ThenCombineEqualCharacteristicsPoints()
        {
            // Arrange.
            IngredientData firstIngredient = _ingredientWithOneCharacteristic;
            IngredientData secondIngredient = _ingredientWithTwoCharacteristics;
            var ingredients = new List<IngredientData>() { firstIngredient, secondIngredient };
            
            // Act.
            yield return CreatePotionInfo(ingredients);
            
            // Assert.
            var createdCharacteristicsAmountPairs = _createdPotion
                .CharacteristicsAmountPairs
                .ToHashSet();

            var expectedCollectionCount = 2;
            
            PotionCharacteristicAmountPair characteristicWithCombinedPoints = createdCharacteristicsAmountPairs.First();

            var charPointsFromFirstIngredient = firstIngredient.CharacteristicAmountPairs.First().PointsAmount;
            var charPointsFromSecondIngredient = secondIngredient.CharacteristicAmountPairs.First().PointsAmount;
            var expectedPointsAmount = charPointsFromFirstIngredient + charPointsFromSecondIngredient;
            
            Assert.That(createdCharacteristicsAmountPairs, Has.Count.EqualTo(expectedCollectionCount));
            Assert.That(characteristicWithCombinedPoints.PointsAmount, Is.EqualTo(expectedPointsAmount));
        }

        private IEnumerator CreatePotionInfo(IEnumerable<IngredientData> ingredients)
        {
            var task = _unitUnderTest.CreatePotionInfoAsync(ingredients);
            yield return task.AsIEnumeratorReturnNull();
            _createdPotion = task.Result;
        }
    }
}