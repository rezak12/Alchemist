using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States
{
    public interface IState : IExitableState
    {
        public UniTask Enter();
    }
}