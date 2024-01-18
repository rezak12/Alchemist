using Code.Data;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.SaveLoadService;
using Code.Infrastructure.Services.SceneLoader;
using Code.UI.AwaitingOverlays;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States.GameStates
{
    public class PotionMakingState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IAssetProvider _assetProvider;
        private readonly IAwaitingOverlay _awaitingOverlay;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPersistentProgressService _progressService;

        public PotionMakingState(
            ISceneLoader sceneLoader, 
            IAssetProvider assetProvider, 
            IAwaitingOverlay awaitingOverlay, 
            ISaveLoadService saveLoadService, 
            IPersistentProgressService progressService)
        {
            _sceneLoader = sceneLoader;
            _assetProvider = assetProvider;
            _awaitingOverlay = awaitingOverlay;
            _saveLoadService = saveLoadService;
            _progressService = progressService;
        }
        public async UniTask Enter() => 
            await _sceneLoader.LoadAsync(ResourcesAddresses.PotionMakingSceneAddress);

        public async UniTask Exit()
        {
            await _awaitingOverlay.Show();
            await _saveLoadService.SaveProgress(_progressService.GetProgress());
            _assetProvider.Cleanup();
        }
    }
}