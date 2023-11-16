using Code.Infrastructure.Services.Pool;
using Code.Infrastructure.Services.StaticData;
using Code.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.SFX
{
    public class SFXProvider : ISFXProvider
    {
        private Pool<SFXPlayer> _pool;
        private IStaticDataService _staticDataService;

        public SFXProvider(SFXPlayer.Factory factory, IStaticDataService staticDataService)
        {
            _pool = new Pool<SFXPlayer>(factory);
            _staticDataService = staticDataService;
        }

        public async UniTask InitializeAsync()
        {
            Transform parent = new GameObject("SFXProvider").transform;
            Object.DontDestroyOnLoad(parent);
            
            PoolObjectConfig poolConfig = _staticDataService.GetPoolConfigByType(PoolObjectType.SFXPlayer);
            await _pool.InitializeAsync(poolConfig.AssetReference, poolConfig.StartCapacity, poolConfig.Type, parent);
        }

        public async UniTask PlayOneShot(AudioClip audioClip, Vector3 position)
        {
            SFXPlayer player = await _pool.Get(position);
            
            await player.PlayOneShot(audioClip);
            
            _pool.Return(player);
        }
    }
}