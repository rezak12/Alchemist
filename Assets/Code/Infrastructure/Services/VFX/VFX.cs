using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.VFX
{
    public class VFX : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        
        public async UniTask Play()
        {
            _particleSystem.Play();
            await UniTask.WaitUntil(() => !_particleSystem.IsAlive());
        }
    }
}