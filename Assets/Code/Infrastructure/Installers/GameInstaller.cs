using Code.Infrastructure.Bootstrappers;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.RandomServices;
using Code.Infrastructure.Services.SaveLoadService;
using Code.Infrastructure.Services.SceneLoader;
using Code.Infrastructure.Services.StaticData;
using Code.Infrastructure.States.GameStates;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindAssetProvider();
            BindPersistentProgressService();
            BindRandomService();
            BindSaveLoadService();
            BindSceneLoader();
            BindStaticDataService();
            BindStatesFactory();
            BindUIFactory();
            BindGameStateMachine();
            BindBootstrapper();
        }

        private void BindAssetProvider()
        {
            Container.BindInterfacesTo<AssetProvider>().AsSingle();
        }

        private void BindPersistentProgressService()
        {
            Container.BindInterfacesTo<PersistentProgressService>().AsSingle();
        }

        private void BindRandomService()
        {
            Container.BindInterfacesTo<UnityRandomService>().AsSingle();
        }

        private void BindSaveLoadService()
        {
            Container.BindInterfacesTo<SaveLoadService>().AsSingle();
        }

        private void BindSceneLoader()
        {
            Container.BindInterfacesTo<SceneLoader>().AsSingle();
        }

        private void BindStaticDataService()
        {
            Container.BindInterfacesTo<StaticDataService>().AsSingle();
        }

        private void BindStatesFactory()
        {
            Container.BindInterfacesTo<StatesFactory>().AsSingle();
        }

        private void BindUIFactory()
        {
            Container.BindInterfacesTo<UIFactory>().AsSingle();
        }

        private void BindGameStateMachine()
        {
            Container.Bind<GameStateMachine>().AsSingle();
        }

        private void BindBootstrapper()
        {
            Container.BindInterfacesAndSelfTo<GameBootstrapper>().AsSingle().NonLazy();
        }
    }
}