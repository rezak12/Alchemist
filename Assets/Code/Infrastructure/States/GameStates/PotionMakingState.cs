using Code.Data;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.SceneLoader;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States.GameStates
{
    public class PotionMakingState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IAssetProvider _assetProvider;

        public PotionMakingState(ISceneLoader sceneLoader, IAssetProvider assetProvider)
        {
            _sceneLoader = sceneLoader;
            _assetProvider = assetProvider;
        }
        public async UniTask Enter()
        {
            await _sceneLoader.LoadAsync(ResourcesPaths.PotionMakingSceneAddress);
        }

        public async UniTask Exit()
        {
            await UniTask.Yield();
            _assetProvider.Cleanup();
        }
    }
}