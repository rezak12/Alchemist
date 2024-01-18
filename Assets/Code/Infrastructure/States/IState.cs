using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States
{
    public interface IState : IExitableState
    {
        UniTask Enter();
    }
}