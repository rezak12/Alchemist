using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.StaticData;
using Code.Logic.Orders;
using Code.UI.AwaitingOverlays;
using Code.UI.SelectionPotionOrderUI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.States.PotionMakingStates
{
    public class OrderSelectionState : IState
    {
        private readonly IUIFactory _uiFactory;
        private readonly IAwaitingOverlay _awaitingOverlay;
        private readonly PotionOrdersHandler _potionOrdersHandler;
        
        private SelectPotionOrderPopup _selectPotionOrderPopup;

        public OrderSelectionState(
            IUIFactory uiFactory, 
            IStaticDataService staticDataService, 
            IPotionOrderFactory potionOrderFactory,
            IAwaitingOverlay awaitingOverlay)
        {
            _uiFactory = uiFactory;
            _awaitingOverlay = awaitingOverlay;
            _potionOrdersHandler = new PotionOrdersHandler(potionOrderFactory, staticDataService);
        }

        public async UniTask Enter()
        {
            await _potionOrdersHandler.HandleNewOrder();
            _selectPotionOrderPopup = await _uiFactory.CreatePotionOrderSelectionPopupAsync(_potionOrdersHandler);
            await _awaitingOverlay.Hide();
        }

        public async UniTask Exit()
        {
            await _awaitingOverlay.Show("Searching for your table...");
            Object.Destroy(_selectPotionOrderPopup.gameObject);
        }
    }
}