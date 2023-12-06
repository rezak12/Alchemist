using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.States.GameStates;
using Code.Logic.Orders;
using Code.UI.Store;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.SelectionPotionOrderUI
{
    public class SelectPotionOrderPopup : MonoBehaviour
    {
        [SerializeField, Range(0, 100)] private int _skipOrderCostInReputation;
        [Space]
        [SerializeField] private OrderDetailsPanel _orderDetailsPanel;
        [SerializeField] private PlayerProgressViewItem _progressViewItem;
        [SerializeField] private Button _openStoreButton;
        [SerializeField] private Button _openMenuButton;
        [SerializeField] private TakeOrderButton _takeOrderButton;
        [SerializeField] private SkipOrderButton _skipOrderButton;
        
        private IUIFactory _uiFactory;
        private GameStateMachine _stateMachine;

        [Inject]
        private void Construct(IUIFactory uiFactory, GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
        }

        public void Initialize(PotionOrdersHandler ordersHandler)
        {
            _orderDetailsPanel.Initialize(ordersHandler);
            _takeOrderButton.Initialize(ordersHandler);
            _skipOrderButton.Initialize(ordersHandler, _skipOrderCostInReputation);
            
            _openStoreButton.onClick.AddListener(OpenStore);
            _openMenuButton.onClick.AddListener(OpenMenu);
        }

        private void OnDestroy()
        {
            _openStoreButton.onClick.RemoveListener(OpenStore);
            _openMenuButton.onClick.RemoveListener(OpenMenu);
        }

        private void OpenStore() => _uiFactory.CreateStorePopupAsync();

        private void OpenMenu() => _stateMachine.Enter<MainMenuState>().Forget();
    }
}