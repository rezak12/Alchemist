using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.SceneLoader;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States.GameStates
{
    public class PotionMakingState : IState
    {
        private const string PotionMakingScene = "PotionMaking";
        
        private readonly ISceneLoader _sceneLoader;
        private readonly IAssetProvider _assetProvider;
        
        private UniTaskCompletionSource _taskCompletionSource;

        public PotionMakingState(ISceneLoader sceneLoader, IAssetProvider assetProvider)
        {
            _sceneLoader = sceneLoader;
            _assetProvider = assetProvider;
        }
        public async UniTask Enter()
        {
            await _sceneLoader.LoadAsync(PotionMakingScene);
        }

        public UniTask Exit()
        {
            _taskCompletionSource = new UniTaskCompletionSource();
            _assetProvider.Cleanup();
            return _taskCompletionSource.Task;
        }
    }
}