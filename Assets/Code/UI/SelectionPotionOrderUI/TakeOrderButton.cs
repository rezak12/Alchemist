using Code.Infrastructure.States.PotionMakingStates;
using Code.Logic.Orders;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.SelectionPotionOrderUI
{
    public class TakeOrderButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private PotionMakingLevelStateMachine _stateMachine;
        private PotionOrdersHandler _potionOrderHandler;
        
        private UnityAction _onButtonClickAction;

        [Inject]
        private void Construct(PotionMakingLevelStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        public void Initialize(PotionOrdersHandler ordersHandler)
        {
            _potionOrderHandler = ordersHandler;
            _onButtonClickAction = UniTask.UnityAction(EnterPotionMakingState);
            
            _button.onClick.AddListener(_onButtonClickAction);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(_onButtonClickAction);
        }

        private async UniTaskVoid EnterPotionMakingState()
        {
            _button.interactable = false;
            await _stateMachine.Enter<OrderStartedState, PotionOrder>(_potionOrderHandler.CurrentOrder);
        }
    }
}