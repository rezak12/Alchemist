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
        private UniTaskCompletionSource _taskCompletionSource;

        public OrderSelectionState(IUIFactory uiFactory, 
            IPotionOrderFactory potionOrderFactory, 
            IStaticDataService staticDataService)
        {
            _uiFactory = uiFactory;
            _potionOrdersHandler = new PotionOrdersHandler(potionOrderFactory, staticDataService);
        }

        public async UniTask Enter()
        {
            _selectPotionOrderPopup = await _uiFactory.CreateSelectPotionOrderPopupAsync(_potionOrdersHandler);
        }

        public UniTask Exit()
        {
            _taskCompletionSource = new UniTaskCompletionSource();
            Object.Destroy(_selectPotionOrderPopup.gameObject);
            return _taskCompletionSource.Task;
        }
    }
}