using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.SFX
{
    public interface ISFXProvider
    {
        UniTask InitializeAsync();
        UniTask PlayOneShot(AudioClip audioClip, Vector3 position);
    }
}