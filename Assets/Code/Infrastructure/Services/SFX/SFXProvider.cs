using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.Pool;
using Code.Infrastructure.Services.StaticData;
using Code.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.SFX
{
    public class SFXProvider : ISFXProvider
    {
        private Pool<SFXPlayer> _pool;
        private IStaticDataService _staticDataService;
        private IAssetProvider _assetProvider;

        public SFXProvider(NonCachePrefabFactory factory, IStaticDataService staticDataService, IAssetProvider assetProvider)
        {
            _pool = new Pool<SFXPlayer>(factory);
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
        }

        public async UniTask InitializeAsync()
        {
            Transform parent = new GameObject("SFXProvider").transform;
            Object.DontDestroyOnLoad(parent);
            
            PoolObjectConfig poolConfig = _staticDataService.GetPoolConfigByType(PoolObjectType.SFXPlayer);
            await _pool.InitializeAsync(poolConfig.AssetReference, poolConfig.StartCapacity, poolConfig.Type, parent);
        }

        public async UniTask PlayOneShot(AssetReferenceT<AudioClip> audioClipReference, Vector3 position)
        {
            SFXPlayer player = await _pool.Get(position);
            var clip = await _assetProvider.LoadAsync<AudioClip>(audioClipReference);
            
            await player.PlayOneShot(clip);
            
            _pool.Return(player);
        }
    }
}