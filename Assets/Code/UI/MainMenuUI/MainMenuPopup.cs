using Code.Infrastructure.States.GameStates;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.MainMenuUI
{
    public class MainMenuPopup : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        private GameStateMachine _stateMachine;

        [Inject]
        private void Construct(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Initialize()
        {
            _playButton.onClick.AddListener(OpenPotionMakingScene);
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OpenPotionMakingScene);
        }

        private void OpenPotionMakingScene()
        {
            _stateMachine.Enter<PotionMakingState>().Forget();
            _playButton.interactable = false;
        }
    }
}