using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States
{
    public interface IExitableState
    { 
        UniTask Exit();
    }
}