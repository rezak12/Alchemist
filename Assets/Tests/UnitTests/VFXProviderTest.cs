using System.Collections;
using System.Linq;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.Pool;
using Code.Infrastructure.Services.StaticData;
using Code.Infrastructure.Services.VFX;
using Code.StaticData;
using Cysharp.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;
using Zenject;
using UnityAssert = UnityEngine.Assertions.Assert;

namespace Tests.UnitTests
{
    [TestFixture]
    public class VFXProviderTest : ZenjectUnitTestFixture
    {
        private VFXProvider _unitUnderTest;
        private const string PoolConfigsPath = "VFXProviderTest";

        public async UniTask CommonInstall()
        {
            var vfxConfigs = Resources.LoadAll<PoolObjectConfig>(PoolConfigsPath)
                .ToDictionary(config => config.Type, config => config);

            var staticDataServiceMock = Substitute.For<IStaticDataService>();
            staticDataServiceMock.GetAllVFXPoolObjectConfigs().Returns(vfxConfigs);

            Container.BindInterfacesTo<AssetProvider>().AsSingle();
            Container
                .BindFactory<AssetReferenceGameObject, UniTask<VFX>, VFX.Factory>()
                .FromFactory<PrefabByReferenceAsyncFactory<VFX>>();

            Container.Bind<VFXProvider>().AsSingle().WithArguments(staticDataServiceMock);
            _unitUnderTest = Container.Resolve<VFXProvider>();

            await _unitUnderTest.InitializeAsync();
        }

        [UnityTest]
        public IEnumerator WhenPoolVFX_AndProviderIsInitialized_ThenReturnCachedObject() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange.
                await CommonInstall();

                // Act.
                VFX pooledObject = await _unitUnderTest.Get(PoolObjectType.PotionVFX, Vector3.zero);

                // Assert.
                Debug.Log(pooledObject.name);
                UnityAssert.IsNotNull(pooledObject);
            });

        [UnityTest]
        public IEnumerator WhenPoolVFX_AndAllObjectsAreInUse_ThenCreateNewAndReturnIt() =>
            UniTask.ToCoroutine(async () =>
        {
            // Arrange.
            await CommonInstall();
            
            // Act.
            // Start Capacity in test config is 1
            VFX objectInUse = await _unitUnderTest.Get(PoolObjectType.PotionVFX, Vector3.zero);
            VFX createdObject = await _unitUnderTest.Get(PoolObjectType.PotionVFX, Vector3.zero);
            
            // Assert.
            UnityAssert.IsNotNull(createdObject);
        });
}
}