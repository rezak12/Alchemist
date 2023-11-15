using Code.Infrastructure.Services.Pool;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

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

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, UniTask<VFX>> { }
    }
}