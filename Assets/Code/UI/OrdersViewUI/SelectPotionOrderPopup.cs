using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.ProgressServices;
using Code.Logic.Orders;
using Code.UI.Store;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.OrdersViewUI
{
    public class SelectPotionOrderPopup : MonoBehaviour
    {
        [SerializeField, Range(0, 100)] private int _skipOrderCostInReputation;
        [Space]
        [SerializeField] private OrderDetailsPanel _orderDetailsPanel;
        [SerializeField] private PlayerProgressViewItem _progressViewItem;
        [SerializeField] private Button _openStoreButton;
        [SerializeField] private TakeOrderButton _takeOrderButton;
        [SerializeField] private SkipOrderButton _skipOrderButton;
        
        private IUIFactory _uiFactory;
        
        [Inject]
        private void Construct(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Initialize(PotionOrdersHandler ordersHandler)
        {
            _orderDetailsPanel.Initialize(ordersHandler);
            _skipOrderButton.Initialize(ordersHandler, _skipOrderCostInReputation);
            
            _openStoreButton.onClick.AddListener(OpenStore);
        }

        private void OnDestroy()
        {
            _openStoreButton.onClick.RemoveListener(OpenStore);
        }
        
        private void OpenStore()
        {
            _uiFactory.CreateStorePopupAsync();
        }
    }
}