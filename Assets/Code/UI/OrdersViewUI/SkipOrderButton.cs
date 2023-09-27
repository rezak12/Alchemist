using Code.Infrastructure.Services.ProgressServices;
using Code.Logic.Orders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.OrdersViewUI
{
    public class SkipOrderButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _skipCostText;
        
        private PotionOrdersHandler _ordersHandler;
        private IPersistentProgressService _progressService;
        private int _skipOrderCostInReputation;

        public void Initialize(
            PotionOrdersHandler ordersHandler, 
            IPersistentProgressService progressService, 
            int skipOrderCostInReputation)
        {
            _skipOrderCostInReputation = skipOrderCostInReputation;
            _skipCostText.text = skipOrderCostInReputation.ToString();
            
            _progressService = progressService;
            _progressService.ReputationAmountChanged += OnReputationAmountChanged;
            
            _ordersHandler = ordersHandler;
            _button.onClick.AddListener(SkipOrder);
        }

        private void OnDestroy()
        {
            _progressService.ReputationAmountChanged -= OnReputationAmountChanged;
        }

        private void SkipOrder()
        {
            _ordersHandler.HandleNewOrder().Forget();
        }

        private void OnReputationAmountChanged()
        {
            _button.interactable = _progressService.ReputationAmount >= _skipOrderCostInReputation;
        }
    }
}