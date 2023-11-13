using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.VFX;
using Code.Infrastructure.States.PotionMakingStates;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Code.Infrastructure.Bootstrappers
{
    public class PotionMakingLevelBootstrapper : IInitializable
    {
        private readonly PotionMakingLevelStateMachine _stateMachine;
        private readonly IStatesFactory _statesFactory;
        private readonly IVFXProvider _vfxProvider;

        public PotionMakingLevelBootstrapper(
            PotionMakingLevelStateMachine stateMachine, 
            IStatesFactory statesFactory,
            IVFXProvider vfxProvider)
        {
            _stateMachine = stateMachine;
            _statesFactory = statesFactory;
            _vfxProvider = vfxProvider;
        }

        public void Initialize()
        {
            _stateMachine.RegisterState(_statesFactory.Create<OrderSelectionState>());
            _stateMachine.RegisterState(_statesFactory.Create<OrderStartedState>());
            _stateMachine.RegisterState(_statesFactory.Create<OrderCompletedState>());

            _vfxProvider.InitializeAsync().Forget();
            
            _stateMachine.Enter<OrderSelectionState>().Forget();
        }
    }
}