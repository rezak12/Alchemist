using Code.Infrastructure.Bootstrappers;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.FX;
using Code.Infrastructure.States.PotionMakingStates;
using Code.Logic.Orders;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class PotionMakingLevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStatesFactory();
            BindSelectedPotionOrderHolder();
            BindAlchemyTableFactory();
            BindIngredientFactory();
            BindPotionInfoFactory();
            BindPotionFactory();
            BindPotionOrderFactory();
            BindUIFactory();
            BindVXFProvider();
            BindStateMachine();
            BindBootstrapper();
        }

        private void BindStatesFactory()
        {
            Container.BindInterfacesTo<StatesFactory>().AsSingle();
        }

        private void BindSelectedPotionOrderHolder()
        {
            Container.BindInterfacesAndSelfTo<SelectedPotionOrderHolder>().AsSingle();
        }

        private void BindAlchemyTableFactory()
        {
            Container.BindInterfacesTo<AlchemyTableFactory>().AsSingle();
        }

        private void BindIngredientFactory()
        {
            Container.BindInterfacesTo<IngredientFactory>().AsSingle();
        }

        private void BindPotionInfoFactory()
        {
            Container.BindInterfacesTo<PotionInfoFactory>().AsSingle();
        }

        private void BindPotionFactory()
        {
            Container.BindInterfacesTo<PotionFactory>().AsSingle();
        }

        private void BindPotionOrderFactory()
        {
            Container.BindInterfacesTo<PotionOrderFactory>().AsSingle();
        }

        private void BindUIFactory()
        {
            Container.BindInterfacesTo<UIFactory>().AsSingle();
        }

        private void BindVXFProvider()
        {
            Container
                .BindFactory<AssetReferenceGameObject, UniTask<VFX>, VFX.Factory>()
                .FromFactory<PrefabByReferenceAsyncFactory<VFX>>();

            Container.BindInterfacesTo<VFXProvider>().AsSingle();
        }

        private void BindStateMachine()
        {
            Container.Bind<PotionMakingLevelStateMachine>().AsSingle();
        }

        private void BindBootstrapper()
        {
            Container.BindInterfacesAndSelfTo<PotionMakingLevelBootstrapper>().AsSingle().NonLazy();
        }
    }
}