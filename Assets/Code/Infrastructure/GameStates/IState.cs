namespace Code.Infrastructure.GameStates
{
    public interface IState : IExitableState
    {
        public void Enter();
    }
}