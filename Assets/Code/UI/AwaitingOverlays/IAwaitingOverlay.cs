using Cysharp.Threading.Tasks;

namespace Code.UI.AwaitingOverlays
{
    public interface IAwaitingOverlay
    {
        public UniTask Show(string message = "Loading...");
        public UniTask Hide();
    }
}