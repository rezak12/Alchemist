using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States
{
    public interface IPayloadState<TPayload> : IExitableState
    {
        public UniTask Enter(TPayload payload);
    }
    
    public interface IPayloadState<TPayload1, TPayload2> : IExitableState
    {
        public UniTask Enter(TPayload1 payload1, TPayload2 payload2);
    }
}