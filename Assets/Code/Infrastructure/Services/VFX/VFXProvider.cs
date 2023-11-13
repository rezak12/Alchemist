using System.Collections.Generic;
using Code.Infrastructure.Services.StaticData;
using Code.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.VFX
{
    public class VFXProvider : IVFXProvider
    {
        private readonly VFX.Factory _factory;
        private readonly IStaticDataService _staticDataService;

        private readonly Dictionary<VFXType, VFXPool> _pools = new();
        
        public VFXProvider(VFX.Factory factory, IStaticDataService staticDataService)
        {
            _factory = factory;
            _staticDataService = staticDataService;
        }

        public async UniTask InitializeAsync()
        {
            Transform parent = new GameObject("VFXProvider").transform;

            var tasks = new List<UniTask>();
            var configs = _staticDataService.GetAllVFXPoolObjectConfigs();
            foreach ((VFXType key, VFXPoolObjectConfig config) in configs)
            {
                var pool = new VFXPool(_factory);
                _pools.Add(key, pool);

                UniTask task = pool.InitializeAsync(config.AssetReference, config.StartCapacity, config.Type, parent);
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask<VFX> Get(VFXType type, Vector3 position)
        {
            return await _pools[type].Get(position);
        }
        
        public void Return(VFXType type, VFX vfx)
        {
            _pools[type].Return(vfx);
        }
    }
}