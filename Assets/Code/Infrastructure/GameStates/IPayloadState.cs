namespace Code.Infrastructure.GameStates
{
    public interface IPayloadState<TPayload> : IExitableState
    {
        public void Enter(TPayload payload);
    }
    
    public interface IPayloadState<TPayload1, TPayload2> : IExitableState
    {
        public void Enter(TPayload1 payload1, TPayload2 payload2);
    }
}