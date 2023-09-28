using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.GameStates
{
    public interface IExitableState
    {
        public UniTask Exit();
    }
}