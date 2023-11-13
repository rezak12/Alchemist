using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Code.Infrastructure.Services.VFX
{
    public class VFX : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        private VFXPool _pool;

        public void Initialize(VFXPool pool)
        {
            _pool = pool;
        }
        
        public async UniTask Play()
        {
            _particleSystem.Play();
            await UniTask.WaitUntil(() => !_particleSystem.IsAlive());
            _pool.Return(this);
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, UniTask<VFX>> { }
    }
}