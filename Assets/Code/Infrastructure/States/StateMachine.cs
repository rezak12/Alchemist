using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States
{
    public abstract class StateMachine
    {
        private readonly Dictionary<System.Type, IExitableState> _registeredStates = new();
        private IExitableState _currentState;

        public async UniTask Enter<TState>() where TState : class, IState
        {
            var newState = await ChangeState<TState>();
            await newState.Enter();
        }

        public async UniTask Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            var newState = await ChangeState<TState>();
            await newState.Enter(payload);
        }
        
        public async UniTask Enter<TState, TPayload1, TPayload2>(TPayload1 payload1, TPayload2 payload2) 
            where TState : class, IPayloadState<TPayload1, TPayload2>
        {
            var newState = await ChangeState<TState>();
            await newState.Enter(payload1, payload2);
        }

        public void RegisterState<TState>(TState state) where TState : IExitableState
        {
            _registeredStates.Add(typeof(TState), state);
        }

        private async UniTask<TState> ChangeState<TState>() where TState : class, IExitableState
        {
            if (_currentState != null)
            {
                await _currentState.Exit();
            }
      
            var state = GetState<TState>();
            _currentState = state;
      
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _registeredStates[typeof(TState)] as TState;
        }
    }
}