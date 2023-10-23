using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.StaticData;
using Code.UI.AwaitingOverlays;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States.GameStates
{
    public class GameBootstrapState : IState
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly AwaitingOverlayProxy _awaitingOverlay;
        private readonly GameStateMachine _stateMachine;

        public GameBootstrapState(
            IAssetProvider assetProvider, 
            IStaticDataService staticDataService,
            AwaitingOverlayProxy awaitingOverlay,
            GameStateMachine stateMachine)
        {
            _awaitingOverlay = awaitingOverlay;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _stateMachine = stateMachine;
        }

        public async UniTask Enter()
        {
            await _assetProvider.InitializeAsync();
            await _awaitingOverlay.InitializeAsync();
            _awaitingOverlay.Show("Loading...").Forget();
            await _staticDataService.InitializeAsync();
            
            await _stateMachine.Enter<LoadProgressState>();
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}