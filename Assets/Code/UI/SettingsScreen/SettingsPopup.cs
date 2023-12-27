using Code.Infrastructure.Services.Settings;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.SettingsScreen
{
    public class SettingsPopup : MonoBehaviour
    {
        [SerializeField] private Slider _sfxVolumeSlider;
        [SerializeField] private Slider _ambientVolumeSlider;
        [SerializeField] private Button _closeButton;

        private ISettingsService _settingsService;

        [Inject]
        private void Construct(ISettingsService settingsService) => _settingsService = settingsService;

        private void OnDestroy() => SaveSettings().Forget();

        private void OnEnable()
        {
            InitializeVolumeSliders();
            
            _sfxVolumeSlider.onValueChanged.AddListener(ChangeSfxVolume);
            _ambientVolumeSlider.onValueChanged.AddListener(ChangeAmbientVolume);
            _closeButton.onClick.AddListener(ClosePopup);
        }

        private void OnDisable()
        {
            _sfxVolumeSlider.onValueChanged.RemoveListener(ChangeSfxVolume);
            _ambientVolumeSlider.onValueChanged.RemoveListener(ChangeAmbientVolume); ;
            _closeButton.onClick.RemoveListener(ClosePopup);
        }

        private void InitializeVolumeSliders()
        {
            _sfxVolumeSlider.value = DecibelToLinear(_settingsService.SfxVolume);
            _ambientVolumeSlider.value = DecibelToLinear(_settingsService.AmbientVolume);
        }

        private void ChangeSfxVolume(float value) => _settingsService.SetSfxVolume(LinearToDecibel(value));
        private void ChangeAmbientVolume(float value) => _settingsService.SetAmbientVolume(LinearToDecibel(value));
        private async UniTaskVoid SaveSettings() => await _settingsService.SaveSettingsAsync();
        private void ClosePopup() => Destroy(gameObject);
        
        /// <summary>
        /// Formula source: https://en.wikipedia.org/wiki/Decibel#Uses
        /// </summary>>
        private float LinearToDecibel(float value) => Mathf.Log10(value) * 20;
        private float DecibelToLinear(float value) => Mathf.Pow(10f, value / 20f);
    }
}