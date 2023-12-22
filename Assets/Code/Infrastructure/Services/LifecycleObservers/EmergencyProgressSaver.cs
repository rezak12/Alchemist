using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.SaveLoadService;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Services.LifecycleObservers
{
    public class EmergencyProgressSaver : MonoBehaviour
    {
        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        [Inject]
        private void Construct(IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _progressService = progressService;
        }
        
        #if UNITY_ANDROID
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveProgress().Forget();
            }
        }
        #endif

        #if UNITY_STANDALONE_WIN
        private void OnApplicationQuit() => SaveProgress().Forget();
        #endif
        
        #if UNITY_EDITOR
        private void OnApplicationQuit() => SaveProgress().Forget();
        #endif

        private async UniTaskVoid SaveProgress() => 
            await _saveLoadService.SaveProgress(_progressService.GetProgress());
    }
}