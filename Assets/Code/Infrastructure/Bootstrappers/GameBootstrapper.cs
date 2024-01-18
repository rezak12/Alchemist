using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.States.GameStates;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Code.Infrastructure.Bootstrappers
{
    public class GameBootstrapper : IInitializable
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IStatesFactory _statesFactory;

        public GameBootstrapper(GameStateMachine stateMachine, IStatesFactory statesFactory)
        {
            _stateMachine = stateMachine;
            _statesFactory = statesFactory;
        }

        public void Initialize()
        {
            _stateMachine.RegisterState(_statesFactory.Create<GameBootstrapState>());
            _stateMachine.RegisterState(_statesFactory.Create<LoadProgressState>());
            _stateMachine.RegisterState(_statesFactory.Create<MainMenuState>());
            _stateMachine.RegisterState(_statesFactory.Create<PotionMakingState>());
            
            _stateMachine.Enter<GameBootstrapState>().Forget();
        }
    }
}