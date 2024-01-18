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
        private void Construct(IPersistentProgressService progressService) => _progressService = progressService;

        public void Initialize(
            PotionOrdersHandler ordersHandler, 
            int skipOrderCostInReputation)
        {
            _skipOrderCostInReputation = skipOrderCostInReputation;
            _skipCostText.text = skipOrderCostInReputation.ToString();
            
            _ordersHandler = ordersHandler;
            
            _skipOrderAction = UniTask.UnityAction(async () => await SkipOrder());
            _button.onClick.AddListener(_skipOrderAction);

            _progressService.ReputationAmountChanged += OnReputationAmountChanged;
            OnReputationAmountChanged();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(_skipOrderAction);
            _progressService.ReputationAmountChanged -= OnReputationAmountChanged;
        }

        private async UniTask SkipOrder()
        {
            _button.interactable = false;
            
            _progressService.RemoveReputation(_skipOrderCostInReputation);
            await _ordersHandler.HandleNewOrder();
        }

        private void OnReputationAmountChanged() => 
            _button.interactable = _progressService.IsReputationEnoughFor(_skipOrderCostInReputation);
    }
}