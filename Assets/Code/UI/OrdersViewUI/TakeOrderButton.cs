using Code.Logic.Orders;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.OrdersViewUI
{
    public class TakeOrderButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private ChosenPotionOrderSender _ordersSender;

        public void Initialize(ChosenPotionOrderSender orderSender)
        {
            _ordersSender = orderSender;
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _ordersSender.SendChosenOrderToGameState();
        }
    }
}