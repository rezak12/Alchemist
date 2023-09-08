using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.RandomServices;
using Code.Infrastructure.Services.SaveLoadService;
using Code.Infrastructure.Services.StaticData;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class ServicesInstaller : MonoInstaller
    {
        [SerializeField] private AssetReference _ingredientItemUIReference;
        [SerializeField] private AssetReference _ingredientCharacteristicItemUIReference;

        public override void InstallBindings()
        {
            BindAssetProvider();
            BindPersistentProgressService();
            BindRandomService();
            BindSaveLoadService();
            BindStaticDataService();
            BindIngredientFactory();
            BindPotionFactory();
            BindPotionOrderFactory();
            BindUIFactory();
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

        private void BindStaticDataService()
        {
            Container.BindInterfacesTo<StaticDataService>().AsSingle();
        }

        private void BindIngredientFactory()
        {
            Container.BindInterfacesTo<IngredientFactory>().AsSingle();
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
            Container.BindInterfacesTo<UIFactory>().AsSingle().WithArguments(
                _ingredientItemUIReference, _ingredientCharacteristicItemUIReference);
        }
    }
}