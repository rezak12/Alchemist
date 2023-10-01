using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.StaticData;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States.GameStates
{
    public class GameBootstrapState : IState
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly GameStateMachine _stateMachine;

        public GameBootstrapState(
            IAssetProvider assetProvider, 
            IStaticDataService staticDataService, 
            GameStateMachine stateMachine)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _stateMachine = stateMachine;
        }

        public async UniTask Enter()
        {
            await UniTask.WhenAll(
                _assetProvider.InitializeAsync(), 
                _staticDataService.InitializeAsync());

            await _stateMachine.Enter<LoadProgressState>();
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}