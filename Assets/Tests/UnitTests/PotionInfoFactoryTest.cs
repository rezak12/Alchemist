using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.Factories;
using Code.Logic.Potions;
using Code.StaticData;
using Code.StaticData.Ingredients;
using Cysharp.Threading.Tasks;
using FluentAssertions;
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
        }

        [UnityTest]
        public IEnumerator
            WhenCreatingPotion_AndPuttedOneIngredientWithOneCharacteristic_ThenPutOnlyThisCharacteristicToPotionInstance() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                IngredientData ingredient = _ingredientWithOneCharacteristic;
                var ingredients = new List<IngredientData>() { ingredient };

                // Act.
                PotionInfo createdPotion = await _unitUnderTest.CreatePotionInfoAsync(ingredients);

                // Assert.
                HashSet<PotionCharacteristicAmountPair> characteristicsPairs = createdPotion.CharacteristicsAmountPairs.ToHashSet();
                characteristicsPairs.Should().HaveCount(1);
            });

        [UnityTest]
        public IEnumerator
            WhenCreatingPotion_AndAllIngredientsHaveUniqueCharacteristics_ThenPutThemAllToPotionInstance() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                const int uniqueCharacteristicsAmount = 3;
                
                IngredientData firstIngredient = _ingredientWithOneCharacteristic;
                IngredientData secondIngredient = _secondIngredientWithTwoCharacteristics;
                var ingredients = new List<IngredientData>() { firstIngredient, secondIngredient };

                // Act.
                PotionInfo createdPotion = await _unitUnderTest.CreatePotionInfoAsync(ingredients);

                // Assert.
                HashSet<PotionCharacteristicAmountPair> characteristicsPairs = createdPotion.CharacteristicsAmountPairs.ToHashSet();
                characteristicsPairs.Should().HaveCount(uniqueCharacteristicsAmount);
            });

        [UnityTest]
        public IEnumerator
            WhenCreatingPotion_AndPuttedIngredientsWithSomeEqualCharacteristics_ThenCombineEqualCharacteristicsPoints() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                const int expectedCharacteristicsAmount = 2;
                
                IngredientData firstIngredient = _ingredientWithOneCharacteristic;
                IngredientData secondIngredient = _ingredientWithTwoCharacteristics;
                var ingredients = new List<IngredientData>() { firstIngredient, secondIngredient };

                // Act.
                PotionInfo createdPotion = await _unitUnderTest.CreatePotionInfoAsync(ingredients);

                // Assert.
                HashSet<PotionCharacteristicAmountPair> characteristicsPairs = createdPotion.CharacteristicsAmountPairs.ToHashSet();

                PotionCharacteristicAmountPair pairWithCombinedPoints = characteristicsPairs.First();

                int pointsFromFirstIngredient = firstIngredient.CharacteristicAmountPairs.First().PointsAmount;
                int pointsFromSecondIngredient = secondIngredient.CharacteristicAmountPairs.First().PointsAmount;
                int expectedPointsAmount = pointsFromFirstIngredient + pointsFromSecondIngredient;

                characteristicsPairs.Should().HaveCount(expectedCharacteristicsAmount);
                pairWithCombinedPoints.PointsAmount.Should().Be(expectedPointsAmount);
            });
    }
}