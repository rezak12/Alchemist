using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.States.PotionMakingStates;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class PotionMakingLevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindAlchemyTableFactory();
            BindIngredientFactory();
            BindPotionInfoFactory();
            BindPotionFactory();
            BindPotionOrderFactory();
            BindStateMachine();
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

        private void BindStateMachine()
        {
            Container.Bind<PotionMakingLevelStateMachine>().AsSingle();
        }
    }
}