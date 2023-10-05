using System.Collections;
using System.Linq;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.RandomServices;
using Code.Infrastructure.Services.SaveLoadService;
using Code.Infrastructure.Services.StaticData;
using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.PotionMakingStates;
using Code.Logic.Orders;
using Code.Logic.PotionMaking;
using Code.Logic.Potions;
using Code.StaticData;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class AlchemyTableTest : ZenjectUnitTestFixture
    {
        private const string FirstIngredientPath = "AlchemyTableTests/Ingredients/FirstIngredient";
        private const string SecondIngredientPath = "AlchemyTableTests/Ingredients/SecondIngredient";
        
        private const string PotionPrefabPath = "Assets/Resources_moved/DevelopmentResources/TestPotion.prefab";
        private const string AlchemyTablePrefabPath = "AlchemyTableTests/TestAlchemyTable";
        
        private IngredientData _firstIngredient;        
        private IngredientData _secondIngredient;
        
        private AlchemyTable _unitUnderTest;
        
        [UnityTest]
        public IEnumerator WhenAddingIngredient_AndTableHaveFreeSlots_ThenFillOneFreeSlot() =>
                UniTask.ToCoroutine(async () =>
                {
                    // Arrange.
                    await CommonInstall();

                    // Act.
                    _unitUnderTest.AddIngredient(_firstIngredient);
                    await UniTask.WaitForSeconds(1);

                    // Assert.
                    Assert.That(_unitUnderTest.IsAllSlotsFree, Is.False);
                });

        [UnityTest]
        public IEnumerator WhenRemovingIngredient_ThenFreeSlot() =>
            UniTask.ToCoroutine(async () =>
            {
                await CommonInstall();

                var filledSlotsAmountChangedInvokeCount = 0;
                _unitUnderTest.FilledSlotsAmountChanged += () => filledSlotsAmountChangedInvokeCount++;

                // Act.
                _unitUnderTest.AddIngredient(_firstIngredient);
                await UniTask.WaitForSeconds(1);
                
                _unitUnderTest.RemoveLastIngredient();
                await UniTask.WaitForSeconds(1);

                // Assert.
                Assert.That(filledSlotsAmountChangedInvokeCount, Is.EqualTo(2));
                Assert.That(_unitUnderTest.IsAllSlotsFree, Is.True);
            });

        [UnityTest]
        public IEnumerator WhenRemovingIngredient_AndAllSlotsAreFree_ThenThrowException() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                await CommonInstall();

                // Act.
                TestDelegate attemptToRemoveIngredient = _unitUnderTest.RemoveLastIngredient;

                // Assert.
                Assert.That(attemptToRemoveIngredient, Throws.Exception);
            });

        [UnityTest]
        public IEnumerator WhenAddingIngredient_AndAllSlotsAreFilled_ThenThrowException() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                await CommonInstall();

                // Act.
                _unitUnderTest.AddIngredient(_secondIngredient);
                await UniTask.WaitForSeconds(1);
                
                _unitUnderTest.AddIngredient(_firstIngredient);
                await UniTask.WaitForSeconds(1);
                
                _unitUnderTest.AddIngredient(_secondIngredient);
                await UniTask.WaitForSeconds(1);
                
                _unitUnderTest.AddIngredient(_firstIngredient);
                await UniTask.WaitForSeconds(1);

                TestDelegate attemptToAddIngredient = () => _unitUnderTest.AddIngredient(_firstIngredient);

                // Assert.
                Assert.That(attemptToAddIngredient, Throws.Exception);
            });

        [UnityTest]
        public IEnumerator WhenHandlingResult_AndAnySlotAreFilled_ThenReleaseAllSlotsAfterHandling() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                await CommonInstall();

                // Act.
                _unitUnderTest.AddIngredient(_secondIngredient);
                await UniTask.WaitForSeconds(1);
                
                _unitUnderTest.HandleResult().Forget();
                await UniTask.WaitForSeconds(1);

                var allSlotReleased = _unitUnderTest.IsAllSlotsFree;

                // Assert.
                Assert.That(allSlotReleased, Is.True);
            });

        [UnityTest]
        public IEnumerator WhenHandlingResult_AndAnySlotAreFilled_ThenCreatePotion() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                await CommonInstall();

                // Act.
                _unitUnderTest.AddIngredient(_secondIngredient);
                await UniTask.WaitForSeconds(1);

                TestDelegate attemptToHandleResult = async () =>
                {
                    _unitUnderTest.HandleResult().Forget();
                    await UniTask.WaitForSeconds(5);
                };

                // Assert.
                Assert.That(attemptToHandleResult, Throws.Nothing);
            });

        [UnityTest]
        public IEnumerator WhenHandlingResult_AndAllSlotsAreFree_ThenCreatePotionWithoutAnyCharacteristic() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                await CommonInstall();

                // Act.
                TestDelegate attemptToHandleResult = async () =>
                {
                    _unitUnderTest.HandleResult().Forget();
                    await UniTask.WaitForSeconds(5);
                };

                // Assert.
                Assert.That(attemptToHandleResult, Throws.Nothing);
            });

        private async UniTask CommonInstall()
        {
            _firstIngredient = Resources.Load<IngredientData>(FirstIngredientPath);
            _secondIngredient = Resources.Load<IngredientData>(SecondIngredientPath);
            var tablePrefab = Resources.Load<AlchemyTable>(AlchemyTablePrefabPath);

            Container.BindInterfacesTo<UnityRandomService>().AsSingle();
            Container.BindInterfacesTo<PersistentProgressService>().AsSingle();
            Container.BindInterfacesTo<SaveLoadService>().AsSingle();
            Container.BindInterfacesTo<AssetProvider>().AsSingle();
            Container.BindInterfacesTo<StaticDataService>().AsSingle();
            Container.BindInterfacesTo<UIFactory>().AsSingle();

            Container.BindInterfacesTo<PotionInfoFactory>().AsSingle();
            Container.BindInterfacesTo<IngredientFactory>().AsSingle();
            Container.BindInterfacesTo<PotionFactory>().AsSingle();

            Container.Bind<SelectedPotionOrderHolder>().AsSingle();
            Container.Bind<GameStateMachine>().AsSingle();
            Container.Bind<PotionMakingLevelStateMachine>().AsSingle();
            Container.Bind<AlchemyTable>().FromComponentInNewPrefab(tablePrefab).AsSingle();

            await Container.Resolve<IAssetProvider>().InitializeAsync();
            await Container.Resolve<IStaticDataService>().InitializeAsync();

            Container.Resolve<IPersistentProgressService>().Initialize(new PlayerProgress(
                0,
                0,
                Enumerable.Empty<string>(),
                AssetDatabase.AssetPathToGUID(PotionPrefabPath),
                string.Empty));

            Container.Resolve<SelectedPotionOrderHolder>().PutOrder(new PotionOrder(
                string.Empty, 
                string.Empty, 
                Enumerable.Empty<PotionCharacteristicAmountPair>(),
                new PotionOrderReward(0,0, null),
                new PotionOrderPunishment(1)));

            Container.Resolve<PotionMakingLevelStateMachine>()
                .RegisterState(Container.Instantiate<OrderCompletedState>());

            _unitUnderTest = Container.Resolve<AlchemyTable>();
            _unitUnderTest.Initialize();
        }
    }
}