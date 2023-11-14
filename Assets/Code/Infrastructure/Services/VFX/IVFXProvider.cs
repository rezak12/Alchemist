using Code.Infrastructure.Services.Pool;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.VFX
{
    public interface IVFXProvider
    {
        UniTask InitializeAsync();
        UniTask<VFX> Get(PoolObjectType type, Vector3 position);
        void Return(PoolObjectType type, VFX vfx);
    }
}