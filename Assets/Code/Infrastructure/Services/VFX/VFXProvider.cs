using System.Collections.Generic;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.Pool;
using Code.Infrastructure.Services.StaticData;
using Code.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.VFX
{
    public class VFXProvider : IVFXProvider
    {
        private readonly CachePrefabFactory _factory;
        private readonly IStaticDataService _staticDataService;

        private readonly Dictionary<PoolObjectType, Pool<VFX>> _pools = new();
        
        public VFXProvider(CachePrefabFactory factory, IStaticDataService staticDataService)
        {
            _factory = factory;
            _staticDataService = staticDataService;
        }

        public async UniTask InitializeAsync()
        {
            Transform parent = new GameObject("VFXProvider").transform;

            List<PoolObjectConfig> configs = new()
            {
                _staticDataService.GetPoolConfigByType(PoolObjectType.IngredientVFX),
                _staticDataService.GetPoolConfigByType(PoolObjectType.PotionVFX)
            };
            
            var tasks = new List<UniTask>(configs.Count);
            foreach (PoolObjectConfig config in configs)
            {
                var pool = new Pool<VFX>(_factory);
                _pools.Add(config.Type, pool);

                UniTask task = pool.InitializeAsync(config.AssetReference, config.StartCapacity, config.Type, parent);
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask Play(PoolObjectType type, Vector3 position)
        {
            Pool<VFX> pool = _pools[type];
            
            VFX vfx = await pool.Get(position);
            await vfx.Play();
            
            pool.Return(vfx);
        }
    }
}