using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.GameStates
{
    public interface IState : IExitableState
    {
        public UniTask Enter();
    }
}