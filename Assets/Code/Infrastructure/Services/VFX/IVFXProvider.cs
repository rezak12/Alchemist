using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.FX
{
    public interface IVFXProvider
    {
        UniTask InitializeAsync();
        UniTask<VFX> Get(VFXType type, Vector3 position);
        void Return(VFXType type, VFX vfx);
    }
}