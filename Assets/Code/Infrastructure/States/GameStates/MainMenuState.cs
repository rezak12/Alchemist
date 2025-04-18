﻿using Code.Data;
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
            await _sceneLoader.LoadAsync(ResourcesAddresses.MainMenuSceneAddress);
            _mainMenuPopup = await _uiFactory.CreateMainMenuPopupAsync();
            _awaitingOverlay.Hide().Forget();
        }

        public async UniTask Exit()
        {
            await _awaitingOverlay.Show();
            Object.Destroy(_mainMenuPopup.gameObject);
        }
    }
}