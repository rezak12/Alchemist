using Code.Infrastructure.States;

namespace Code.Infrastructure.Services.Factories
{
    public interface IStatesFactory
    {
        public TState Create<TState>() where TState : IExitableState;
    }
}