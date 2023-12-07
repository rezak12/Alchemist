using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States
{
    public interface IPayloadState<TPayload> : IExitableState
    {
        UniTask Enter(TPayload payload);
    }
    
    public interface IPayloadState<TPayload1, TPayload2> : IExitableState
    {
        UniTask Enter(TPayload1 payload1, TPayload2 payload2);
    }
}