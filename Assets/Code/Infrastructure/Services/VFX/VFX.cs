using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Code.Infrastructure.Services.FX
{
    public class VFX : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<AssetReferenceGameObject, UniTask<VFX>> { }
    }
}