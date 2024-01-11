using Code.Infrastructure.Services.StaticData;
using Code.Infrastructure.States.GameStates;
using Code.StaticData;
using Code.StaticData.Configs;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.MainMenuUI
{
    public class MainMenuPopup : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private TextMeshProUGUI _versionText;
        [SerializeField] private TextMeshProUGUI _labelText;
        [SerializeField] private TextMeshProUGUI _additionInformationText;
        
        private GameStateMachine _stateMachine;
        private IStaticDataService _staticDataService;

        [Inject]
        private void Construct(GameStateMachine stateMachine, IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _stateMachine = stateMachine;
        }

        public void Initialize()
        {
            _playButton.onClick.AddListener(OpenPotionMakingScene);
            SetVersionInfo();
        }

        private void OnDestroy() => _playButton.onClick.RemoveListener(OpenPotionMakingScene);

        private void OpenPotionMakingScene()
        {
            _stateMachine.Enter<PotionMakingState>().Forget();
            _playButton.interactable = false;
        }

        private void SetVersionInfo()
        {
            VersionInfo versionInfo = _staticDataService.GetVersionInfo();
            _versionText.text = versionInfo.Version;
            _labelText.text = versionInfo.Label;
            _additionInformationText.text = versionInfo.AdditionInformation;
        }
    }
}