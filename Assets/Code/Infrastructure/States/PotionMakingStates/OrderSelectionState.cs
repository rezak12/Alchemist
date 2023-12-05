using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.StaticData;
using Code.Infrastructure.Services.VFX;
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
        private readonly IVFXProvider _vfxProvider;
        
        private SelectPotionOrderPopup _selectPotionOrderPopup;

        public OrderSelectionState(
            IUIFactory uiFactory, 
            IStaticDataService staticDataService, 
            IPotionOrderFactory potionOrderFactory,
            IAwaitingOverlay awaitingOverlay, 
            IVFXProvider vfxProvider)
        {
            _uiFactory = uiFactory;
            _awaitingOverlay = awaitingOverlay;
            _vfxProvider = vfxProvider;
            _potionOrdersHandler = new PotionOrdersHandler(potionOrderFactory, staticDataService);
        }

        public async UniTask Enter()
        {
            await _potionOrdersHandler.HandleNewOrder();
            _selectPotionOrderPopup = await _uiFactory.CreateSelectPotionOrderPopupAsync(_potionOrdersHandler);
            await _vfxProvider.InitializeAsync();
            await _awaitingOverlay.Hide();
        }

        public async UniTask Exit()
        {
            await _awaitingOverlay.Show("Searching for your table...");
            Object.Destroy(_selectPotionOrderPopup.gameObject);
        }
    }
}