using Code.Logic.Orders;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.OrdersViewUI
{
    public class TakeOrderButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private PotionOrdersHandler _ordersHandler;

        public void Initialize(PotionOrdersHandler ordersHandler)
        {
            _ordersHandler = ordersHandler;
        }
    }
}