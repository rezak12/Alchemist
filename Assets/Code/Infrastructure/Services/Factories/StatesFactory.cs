using Code.Infrastructure.States;
using Zenject;

namespace Code.Infrastructure.Services.Factories
{
    public class StatesFactory : IStatesFactory
    {
        private readonly IInstantiator _instantiator;

        public StatesFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }
        
        public TState Create<TState>() where TState : IExitableState
        {
            return _instantiator.Instantiate<TState>();
        }
    }
}