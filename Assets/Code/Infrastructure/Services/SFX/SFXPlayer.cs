using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.SFX
{
    public class SfxPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        
        private CancellationToken _cancellationToken;
        private Func<bool> _waitUntilPredicate;

        private void Awake()
        {
            _cancellationToken = this.GetCancellationTokenOnDestroy();
            _waitUntilPredicate = () => !_audioSource.isPlaying;
        }

        public async UniTask PlayOneShot(AudioClip audioClip)
        {
            _audioSource.PlayOneShot(audioClip);
            await UniTask.WaitUntil(_waitUntilPredicate, cancellationToken: _cancellationToken);
        }
    }
}