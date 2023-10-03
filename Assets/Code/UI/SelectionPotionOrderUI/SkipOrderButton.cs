using Code.Infrastructure.Services.ProgressServices;
using Code.Logic.Orders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.SelectionPotionOrderUI
{
    public class SkipOrderButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _skipCostText;
        
        private PotionOrdersHandler _ordersHandler;
        private IPersistentProgressService _progressService;
        private int _skipOrderCostInReputation;

        [Inject]
        private void Construct(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }

        public void Initialize(
            PotionOrdersHandler ordersHandler, 
            int skipOrderCostInReputation)
        {
            _skipOrderCostInReputation = skipOrderCostInReputation;
            _skipCostText.text = skipOrderCostInReputation.ToString();
            
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