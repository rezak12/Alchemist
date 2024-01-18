using Cysharp.Threading.Tasks;
using UnityEngine.Audio;

namespace Code.Infrastructure.Services.Settings
{
    public interface ISettingsService
    {
        UniTask InitializeAsync(AudioMixer audioMixer);
        float AmbientVolume { get; }
        float SfxVolume { get; }

        void SetAmbientVolume(float value);
        void SetSfxVolume(float value);
        UniTask SaveSettingsAsync();
    }
}