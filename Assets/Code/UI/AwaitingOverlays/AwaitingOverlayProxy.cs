using Code.Infrastructure.Services.Factories;
using Cysharp.Threading.Tasks;

namespace Code.UI.AwaitingOverlays
{
    public class AwaitingOverlayProxy : IAwaitingOverlay
    {
        private readonly IUIFactory _uiFactory;
        private IAwaitingOverlay _overlay;

        public AwaitingOverlayProxy(IUIFactory factory)
        {
            _uiFactory = factory;
        }

        public async UniTask InitializeAsync()
        {
            await _uiFactory.CreateAwaitingOverlay();
        }

        public void Show(string message = "")
        {
            _overlay.Show(message);
        }

        public void Hide()
        {
            _overlay.Hide();
        }
    }
}