using Code.Infrastructure.Bootstrappers;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.VFX;
using Code.Infrastructure.States.PotionMakingStates;
using Code.Logic.Orders;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class PotionMakingLevelInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;

        public override void InstallBindings()
        {
            BindCamera();
            BindStatesFactory();
            BindSelectedPotionOrderHolder();
            BindAlchemyTableFactory();
            BindPotionInfoFactory();
            BindPotionFactory();
            BindPotionOrderFactory();
            BindUIFactory();
            BindVXFProvider();
            BindStateMachine();
            BindBootstrapper();
        }

        private void BindCamera() => Container.Bind<Camera>().FromInstance(_camera);

        private void BindStatesFactory() => Container.BindInterfacesTo<StatesFactory>().AsSingle();

        private void BindSelectedPotionOrderHolder() => 
            Container.BindInterfacesAndSelfTo<SelectedPotionOrderHolder>().AsSingle();

        private void BindAlchemyTableFactory() => Container.BindInterfacesTo<AlchemyTableFactory>().AsSingle();
        
        private void BindPotionInfoFactory() => Container.BindInterfacesTo<PotionInfoFactory>().AsSingle();

        private void BindPotionFactory() => Container.BindInterfacesTo<PotionFactory>().AsSingle();

        private void BindPotionOrderFactory() => Container.BindInterfacesTo<PotionOrderFactory>().AsSingle();

        private void BindUIFactory() => Container.BindInterfacesTo<UIFactory>().AsSingle();

        private void BindVXFProvider() => Container.BindInterfacesTo<VFXProvider>().AsSingle();

        private void BindStateMachine() => Container.Bind<PotionMakingLevelStateMachine>().AsSingle();

        private void BindBootstrapper() => 
            Container.BindInterfacesAndSelfTo<PotionMakingLevelBootstrapper>().AsSingle().NonLazy();
    }
}