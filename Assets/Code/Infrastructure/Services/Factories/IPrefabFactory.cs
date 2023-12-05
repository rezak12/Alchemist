using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.Factories
{
    public interface IPrefabFactory
    { 
        UniTask<TComponent> Create<TComponent>(string key) where TComponent : MonoBehaviour;
        UniTask<TComponent> Create<TComponent>(AssetReference reference) where TComponent : MonoBehaviour;
    }
}