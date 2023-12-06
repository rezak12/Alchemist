using Code.Data;
using Code.Infrastructure.Services.Factories;
using Cysharp.Threading.Tasks;

namespace Code.UI.AwaitingOverlays
{
    public class AwaitingOverlayProxy : IAwaitingOverlay
    {
        private IAwaitingOverlay _overlay;
        private readonly IPrefabFactory _factory;

        public AwaitingOverlayProxy(NonCachePrefabFactory factory) => _factory = factory;

        public async UniTask InitializeAsync() => 
            _overlay = await _factory.Create<AwaitingOverlay>(ResourcesAddresses.AwaitingOverlayAddress);

        public async UniTask Show(string message = "Loading...") => await _overlay.Show(message);

        public async UniTask Hide() => await _overlay.Hide();
    }
}