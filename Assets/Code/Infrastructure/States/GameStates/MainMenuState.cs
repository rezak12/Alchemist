using Code.Data;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.SceneLoader;
using Code.UI.AwaitingOverlays;
using Code.UI.MainMenuUI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.States.GameStates
{
    public class MainMenuState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private readonly IAwaitingOverlay _awaitingOverlay;
        
        private MainMenuPopup _mainMenuPopup;

        public MainMenuState(ISceneLoader sceneLoader, IUIFactory uiFactory, IAwaitingOverlay awaitingOverlay)
        {
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _awaitingOverlay = awaitingOverlay;
        }

        public async UniTask Enter()
        {
            await _sceneLoader.LoadAsync(ResourcesPaths.MainMenuSceneAddress);
            _mainMenuPopup = await _uiFactory.CreateMainMenuPopupAsync();
            _awaitingOverlay.Hide().Forget();
        }

        public UniTask Exit()
        {
            _awaitingOverlay.Show("Loading...").Forget();
            Object.Destroy(_mainMenuPopup.gameObject);
            return UniTask.CompletedTask;
        }
    }
}