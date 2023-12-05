using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.States.PotionMakingStates;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Code.Infrastructure.Bootstrappers
{
    public class PotionMakingLevelBootstrapper : IInitializable
    {
        private readonly PotionMakingLevelStateMachine _stateMachine;
        private readonly IStatesFactory _statesFactory;
        
        public PotionMakingLevelBootstrapper(PotionMakingLevelStateMachine stateMachine, IStatesFactory statesFactory)
        {
            _stateMachine = stateMachine;
            _statesFactory = statesFactory;
        }

        public void Initialize()
        {
            _stateMachine.RegisterState(_statesFactory.Create<OrderSelectionState>());
            _stateMachine.RegisterState(_statesFactory.Create<OrderStartedState>());
            _stateMachine.RegisterState(_statesFactory.Create<OrderCompletedState>());
            
            _stateMachine.Enter<OrderSelectionState>().Forget();
        }
    }
}