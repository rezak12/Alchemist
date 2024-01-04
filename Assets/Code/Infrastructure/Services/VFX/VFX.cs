using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.VFX
{
    public class VFX : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        
        private CancellationToken _cancellationToken;
        private Func<bool> _waitUntilPredicate;

        private void Awake()
        {
            _waitUntilPredicate = () => !_particleSystem.IsAlive();
            _cancellationToken = this.GetCancellationTokenOnDestroy();
        }

        public async UniTask Play()
        {
            _particleSystem.Play();
            await UniTask.WaitUntil(_waitUntilPredicate, cancellationToken: _cancellationToken);
        }
    }
}