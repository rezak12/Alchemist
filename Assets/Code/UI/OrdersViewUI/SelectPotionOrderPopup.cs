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
        private IPersistentProgressService _progressService;
        
        [Inject]
        private void Construct(IUIFactory uiFactory, IPersistentProgressService progressService)
        {
            _uiFactory = uiFactory;
            _progressService = progressService;
        }

        public void Initialize(PotionOrdersHandler ordersHandler, ChosenPotionOrderSender orderSender)
        {
            _orderDetailsPanel.Initialize(ordersHandler, _uiFactory);
            _progressViewItem.Initialize(_progressService);
            _takeOrderButton.Initialize(orderSender);
            _skipOrderButton.Initialize(ordersHandler, _progressService, _skipOrderCostInReputation);
            
            _openStoreButton.onClick.AddListener(OpenStore);
        }

        private void OnDestroy()
        {
            _openStoreButton.onClick.RemoveListener(OpenStore);
        }
        
        private void OpenStore()
        {
            _uiFactory.CreateStoreWindow();
        }
    }
}