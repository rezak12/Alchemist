using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.States.GameStates;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Bootstrappers
{
    public class GameBootstrapper : MonoBehaviour
    {
        private GameStateMachine _stateMachine;
        private IStatesFactory _statesFactory;

        [Inject]
        private void Construct(GameStateMachine stateMachine, IStatesFactory statesFactory)
        {
            _stateMachine = stateMachine;
            _statesFactory = statesFactory;
        }

        private void Awake()
        {
            _stateMachine.RegisterState(_statesFactory.Create<GameBootstrapState>());
            _stateMachine.RegisterState(_statesFactory.Create<LoadProgressState>());
            _stateMachine.RegisterState(_statesFactory.Create<MainMenuState>());
            _stateMachine.RegisterState(_statesFactory.Create<PotionMakingState>());
            
            _stateMachine.Enter<GameBootstrapState>().Forget();
        }
    }
}