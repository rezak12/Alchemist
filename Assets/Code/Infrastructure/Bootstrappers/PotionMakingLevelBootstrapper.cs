using System;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.PotionMakingStates;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Bootstrappers
{
    public class PotionMakingLevelBootstrapper : MonoBehaviour
    {
        private PotionMakingLevelStateMachine _stateMachine;
        private IStatesFactory _statesFactory;

        [Inject]
        private void Construct(PotionMakingLevelStateMachine stateMachine, IStatesFactory statesFactory)
        {
            _stateMachine = stateMachine;
            _statesFactory = statesFactory;
        }

        private void Awake()
        {
            _stateMachine.RegisterState(_statesFactory.Create<OrderSelectionState>());
            _stateMachine.RegisterState(_statesFactory.Create<OrderStartedState>());
            _stateMachine.RegisterState(_statesFactory.Create<OrderCompletedState>());

            _stateMachine.Enter<OrderSelectionState>().Forget();
        }
    }
}