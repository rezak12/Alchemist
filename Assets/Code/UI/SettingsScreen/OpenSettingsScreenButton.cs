
using Code.Infrastructure.Services.Factories;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.SettingsScreen
{
    public class OpenSettingsScreenButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private UnityAction _openScreenAction;
        
        private IUIFactory _uiFactory;

        [Inject]
        private void Construct(IUIFactory uiFactory) => _uiFactory = uiFactory;

        private void OnEnable()
        {
            _openScreenAction = UniTask.UnityAction(OpenScreen);
            _button.onClick.AddListener(_openScreenAction);
        }

        private void OnDisable() => _button.onClick.RemoveListener(_openScreenAction);

        private async UniTaskVoid OpenScreen()
        {
            _button.interactable = false;
            await _uiFactory.CreateSettingsScreen();
            _button.interactable = true;

        }
    }
}