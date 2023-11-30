using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Code.Infrastructure.Services.SFX
{
    public class SFXPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        public async UniTask PlayOneShot(AudioClip audioClip)
        {
            _audioSource.PlayOneShot(audioClip);
            await UniTask.WaitUntil(() => !_audioSource.isPlaying);
        }
    }
}