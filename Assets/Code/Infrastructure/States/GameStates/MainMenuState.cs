using Code.Data;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.SceneLoader;
using Code.UI.MainMenuUI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.States.GameStates
{
    public class MainMenuState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private MainMenuPopup _mainMenuPopup;

        public MainMenuState(ISceneLoader sceneLoader, IUIFactory uiFactory)
        {
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
        }

        public async UniTask Enter()
        {
            await _sceneLoader.LoadAsync(ResourcesPaths.MainMenuSceneAddress);
            _mainMenuPopup = await _uiFactory.CreateMainMenuPopupAsync();
        }

        public async UniTask Exit()
        {
            await UniTask.Yield();
            Object.Destroy(_mainMenuPopup.gameObject);
        }
    }
}