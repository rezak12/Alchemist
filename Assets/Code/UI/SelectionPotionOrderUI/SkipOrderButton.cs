using Code.Infrastructure.Services.ProgressServices;
using Code.Logic.Orders;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
        private UnityAction _skipOrderAction;

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
            
            _skipOrderAction = UniTask.UnityAction(async () => await SkipOrder());
            _button.onClick.AddListener(_skipOrderAction);
        }

        private void OnDestroy()
        {
            _progressService.ReputationAmountChanged -= OnReputationAmountChanged;
            _button.onClick.RemoveListener(_skipOrderAction);
        }

        private async UniTask SkipOrder()
        {
            _button.interactable = false;
            await _ordersHandler.HandleNewOrder();
            _button.interactable = true;
        }

        private void OnReputationAmountChanged()
        {
            _button.interactable = _progressService.ReputationAmount >= _skipOrderCostInReputation;
        }
    }
}