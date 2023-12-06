using Code.Infrastructure.Services.VFX;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States.PotionMakingStates
{
    public class PotionMakingLevelBootstrapState : IState
    {
        private readonly IVFXProvider _vfxProvider;
        private readonly PotionMakingLevelStateMachine _stateMachine;

        public PotionMakingLevelBootstrapState(IVFXProvider vfxProvider, PotionMakingLevelStateMachine stateMachine)
        {
            _vfxProvider = vfxProvider;
            _stateMachine = stateMachine;
        }

        public async UniTask Enter()
        {
            await _vfxProvider.InitializeAsync();
            await _stateMachine.Enter<OrderSelectionState>();
        }

        public UniTask Exit() => UniTask.CompletedTask;
    }
}