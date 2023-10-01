using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.SceneLoader;
using Code.Infrastructure.Services.StaticData;
using Code.Logic.Orders;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States.GameStates
{
    public class OrderSelectionState : IState
    {
        private const string OrderSelectionSceneAddress = "SelectPotionOrder";
        
        private readonly IUIFactory _uiFactory;
        private readonly ISceneLoader _sceneLoader;
        private readonly PotionOrdersHandler _potionOrdersHandler;


        public OrderSelectionState(IUIFactory uiFactory, 
            ISceneLoader sceneLoader, 
            IPotionOrderFactory potionOrderFactory, 
            IStaticDataService staticDataService)
        {
            _uiFactory = uiFactory;
            _sceneLoader = sceneLoader;
            _potionOrdersHandler = new PotionOrdersHandler(potionOrderFactory, staticDataService);
        }

        public async UniTask Enter()
        {
            await _sceneLoader.LoadAsync(OrderSelectionSceneAddress);
            await _uiFactory.CreateSelectPotionOrderPopupAsync(_potionOrdersHandler);
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}