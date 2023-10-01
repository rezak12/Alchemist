using Code.Infrastructure.States.GameStates;
using Code.Logic.Orders;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.OrdersViewUI
{
    public class TakeOrderButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private GameStateMachine _stateMachine;

        [Inject]
        private void Construct(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        private void Start()
        {
            _button.onClick.AddListener(EnterPotionMakingState);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(EnterPotionMakingState);
        }

        private void EnterPotionMakingState()
        {
            // _stateMachine;
        }
    }
}