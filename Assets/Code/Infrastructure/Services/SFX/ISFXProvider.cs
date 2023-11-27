using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.SFX
{
    public interface ISFXProvider
    {
        UniTask InitializeAsync();
        UniTask PlayOneShot(AssetReferenceT<AudioClip> audioClipReference, Vector3 position);
    }
}