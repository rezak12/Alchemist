using Code.Data;
using Code.Infrastructure.Services.SaveLoadService;
using Cysharp.Threading.Tasks;
using UnityEngine.Audio;

namespace Code.Infrastructure.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        public float AmbientVolume => _settings.VolumeSettings.AmbientVolume;
        public float SfxVolume => _settings.VolumeSettings.SfxVolume;
        
        private AudioMixer _audioMixer;
        private GameSettings _settings;

        private readonly ISaveLoadService _saveLoadService;

        public SettingsService(ISaveLoadService saveLoadService) => _saveLoadService = saveLoadService;

        public async UniTask InitializeAsync(AudioMixer audioMixer)
        {
            _audioMixer = audioMixer;

            _settings = await _saveLoadService.LoadSettings();
            if (_settings == null)
            {
                _settings = CreateGameSettings();
                return;
            }

            InitializeAudioMixer(_settings.VolumeSettings);
        }

        public void SetAmbientVolume(float value)
        {
            _audioMixer.SetFloat(Constants.AmbientVolumeParameter, value);
            _settings.VolumeSettings.AmbientVolume = value;
        }

        public void SetSfxVolume(float value)
        {
            _audioMixer.SetFloat(Constants.SfxVolumeParameter, value);
            _settings.VolumeSettings.SfxVolume = value;
        }

        public UniTask SaveSettingsAsync() => _saveLoadService.SaveSettings(_settings);

        private void InitializeAudioMixer(VolumeSettings volumeSettings)
        {
            _audioMixer.SetFloat(Constants.AmbientVolumeParameter, volumeSettings.AmbientVolume);
            _audioMixer.SetFloat(Constants.SfxVolumeParameter, volumeSettings.SfxVolume);
        }

        private GameSettings CreateGameSettings() => new(CreateVolumeSettings());

        private VolumeSettings CreateVolumeSettings()
        {
            var settings = new VolumeSettings();
            _audioMixer.GetFloat(Constants.AmbientVolumeParameter, out settings.AmbientVolume);
            _audioMixer.GetFloat(Constants.SfxVolumeParameter, out settings.SfxVolume);
            return settings;
        }
    }
}