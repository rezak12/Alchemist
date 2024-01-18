using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Code.Infrastructure.Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        public async UniTask LoadAsync(string sceneName)
        {
            AsyncOperationHandle<SceneInstance> handler = Addressables.LoadSceneAsync(
                sceneName, 
                LoadSceneMode.Single, 
                false);

            await handler.ToUniTask();
            await handler.Result.ActivateAsync().ToUniTask();
        }
    }
}