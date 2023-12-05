using Code.Data;
using Code.Infrastructure.Services.AssetProvider;
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

        public PotionMakingState(
            ISceneLoader sceneLoader, 
            IAssetProvider assetProvider, 
            IAwaitingOverlay awaitingOverlay)
        {
            _sceneLoader = sceneLoader;
            _assetProvider = assetProvider;
            _awaitingOverlay = awaitingOverlay;
        }
        public async UniTask Enter()
        {
            await _sceneLoader.LoadAsync(ResourcesAddresses.PotionMakingSceneAddress);
        }

        public async UniTask Exit()
        {
            await _awaitingOverlay.Show();
            _assetProvider.Cleanup();
        }
    }
}