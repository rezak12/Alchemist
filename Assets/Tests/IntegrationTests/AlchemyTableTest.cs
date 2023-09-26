using System.Collections;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.ProgressServices;
using Code.Logic.PotionMaking;
using Code.StaticData;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityAssert = UnityEngine.Assertions.Assert;
using Zenject;

namespace Tests.IntegrationTests
{
    public class AlchemyTableTest : ZenjectIntegrationTestFixture
    {
        private const string FirstIngredientPath = "AlchemyTableTests/Ingredients/FirstIngredient";
        private const string SecondIngredientPath = "AlchemyTableTests/Ingredients/SecondIngredient";
        
        private const string AlchemyTablePrefabPath = "AlchemyTableTests/TestAlchemyTable";
        
        private IngredientData _firstIngredient;        
        private IngredientData _secondIngredient;
        
        private AlchemyTable _unitUnderTest;

        [UnityTest]
        public IEnumerator WhenAddingIngredient_AndTableHaveFreeSlots_ThenFillOneFreeSlot()
        {
            // Arrange.
            CommonInstall();

            var filledSlotsAmountChangedEventWasInvoked = false;
            _unitUnderTest.FilledSlotsAmountChanged += () => filledSlotsAmountChangedEventWasInvoked = true;
            
            // Act.
            yield return null;
            _unitUnderTest.AddIngredient(_firstIngredient);
            
            // Assert.
            Assert.That(filledSlotsAmountChangedEventWasInvoked, Is.True);
        }

        [UnityTest]
        public IEnumerator WhenRemovingIngredient_ThenFreeSlot()
        {
            CommonInstall();

            var filledSlotsAmountChangedInvokeCount = 0;
            _unitUnderTest.FilledSlotsAmountChanged += () => filledSlotsAmountChangedInvokeCount++;
            
            // Act.
            yield return null;
            _unitUnderTest.AddIngredient(_firstIngredient);
            _unitUnderTest.RemoveLastIngredient();
            
            // Assert.
            Assert.That(filledSlotsAmountChangedInvokeCount, Is.EqualTo(2));
            Assert.That(_unitUnderTest.IsAllSlotsFree, Is.True);
        }

        [UnityTest]
        public IEnumerator WhenRemovingIngredient_AndAllSlotsAreFree_ThenThrowException()
        {
            // Arrange.
            CommonInstall();
            
            // Act.
            yield return null;
            TestDelegate attemptToRemoveIngredient = _unitUnderTest.RemoveLastIngredient;
            
            // Assert.
            Assert.That(attemptToRemoveIngredient, Throws.Exception);
        }

        [UnityTest]
        public IEnumerator WhenAddingIngredient_AndAllSlotsAreFilled_ThenThrowException()
        {
            // Arrange.
            CommonInstall();
            
            // Act.
            yield return null;
            
            _unitUnderTest.AddIngredient(_secondIngredient);
            _unitUnderTest.AddIngredient(_firstIngredient);
            _unitUnderTest.AddIngredient(_secondIngredient);
            _unitUnderTest.AddIngredient(_firstIngredient);
            
            TestDelegate attemptToAddIngredient = () => _unitUnderTest.AddIngredient(_firstIngredient);
            
            // Assert.
            Assert.That(attemptToAddIngredient, Throws.Exception);
        }

        [UnityTest]
        public IEnumerator WhenHandlingResult_AndAnySlotAreFilled_ThenCreatePotion()
        {
            // Arrange.
            CommonInstall();
            
            // Act.
            yield return null;
            _unitUnderTest.AddIngredient(_secondIngredient);
            TestDelegate attemptToHandleResult = _unitUnderTest.HandleResult;

            // Assert.
            Assert.That(attemptToHandleResult, Throws.Nothing);
        }

        [UnityTest]
        public IEnumerator WhenHandlingResult_AndAnySlotAreFilled_ThenReleaseAllSlotsAfterHandling()
        {
            // Arrange.
            CommonInstall();
            
            // Act.
            yield return null;
            _unitUnderTest.AddIngredient(_secondIngredient);
            _unitUnderTest.HandleResult();

            var allSlotReleased = _unitUnderTest.IsAllSlotsFree;

            // Assert.
            Assert.That(allSlotReleased, Is.True);
        }
        
        [UnityTest] public IEnumerator WhenHandlingResult_AndAllSlotsAreFree_ThenThrowException()
        {
            // Arrange.
            CommonInstall();
            
            // Act.
            yield return null;
            TestDelegate attemptToHandleResult = _unitUnderTest.HandleResult;

            // Assert.
            Assert.That(attemptToHandleResult, Throws.Exception);
        }

        private void CommonInstall()
        {
            _firstIngredient = Resources.Load<IngredientData>(FirstIngredientPath);
            _secondIngredient = Resources.Load<IngredientData>(SecondIngredientPath);
            
            var tablePrefab = Resources.Load<AlchemyTable>(AlchemyTablePrefabPath);
            
            PreInstall();
            
            Container.BindInterfacesTo<PersistentProgressService>().AsSingle();
            Container.BindInterfacesTo<AssetProvider>().AsSingle();

            Container.BindInterfacesTo<PotionInfoFactory>().AsSingle();
            Container.BindInterfacesTo<IngredientFactory>().AsSingle();
            Container.BindInterfacesTo<PotionFactory>().AsSingle();

            Container.Bind<AlchemyTable>().FromComponentInNewPrefab(tablePrefab).AsSingle();

            PostInstall();

            _unitUnderTest = Container.Resolve<AlchemyTable>();
            _unitUnderTest.Initialize();
        }
    }
}