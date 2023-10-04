using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.StaticData;
using Code.Logic.Orders;
using Code.UI.SelectionPotionOrderUI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.States.PotionMakingStates
{
    public class OrderSelectionState : IState
    {
        private readonly IUIFactory _uiFactory;
        private readonly PotionOrdersHandler _potionOrdersHandler;
        
        private SelectPotionOrderPopup _selectPotionOrderPopup;

        public OrderSelectionState(IUIFactory uiFactory, 
            IStaticDataService staticDataService, 
            IPotionOrderFactory potionOrderFactory)
        {
            _uiFactory = uiFactory;
            _potionOrdersHandler = new PotionOrdersHandler(potionOrderFactory, staticDataService);
        }

        public async UniTask Enter()
        {
            await _potionOrdersHandler.HandleNewOrder();
            _selectPotionOrderPopup = await _uiFactory.CreateSelectPotionOrderPopupAsync(_potionOrdersHandler);
        }

        public async UniTask Exit()
        {
            await UniTask.Yield();
            Object.Destroy(_selectPotionOrderPopup.gameObject);
        }
    }
}