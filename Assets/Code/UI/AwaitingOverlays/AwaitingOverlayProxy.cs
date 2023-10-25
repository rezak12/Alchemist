using Code.Data;
using Cysharp.Threading.Tasks;

namespace Code.UI.AwaitingOverlays
{
    public class AwaitingOverlayProxy : IAwaitingOverlay
    {
        private readonly AwaitingOverlay.Factory _uiFactory;
        private IAwaitingOverlay _overlay;

        public AwaitingOverlayProxy(AwaitingOverlay.Factory factory)
        {
            _uiFactory = factory;
        }

        public async UniTask InitializeAsync()
        {
            _overlay = await _uiFactory.Create(ResourcesPaths.AwaitingOverlayAddress);
        }

        public async UniTask Show(string message = "Loading...")
        {
            await _overlay.Show(message);
        }

        public async UniTask Hide()
        {
            await _overlay.Hide();
        }
    }
}