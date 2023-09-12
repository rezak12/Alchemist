using System.Collections;
using System.Threading.Tasks;
using Code.Infrastructure.Services.Factories;
using Code.Logic.Orders;
using Code.UI.PotionCharacteristicsUI;
using TMPro;
using UnityEngine;

namespace Code.UI.OrdersViewUI
{
    public class OrderDetailsPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _orderDifficultyLevelNameText;
        [SerializeField] private TextMeshProUGUI _orderTypeNameText;
        [SerializeField] private PotionCharacteristicItemsContainer _requirementCharacteristicsContainer;
        [SerializeField] private PotionOrderRewardItemUI _rewardItem;
        
        private PotionOrdersHandler _ordersHandler;
        private IUIFactory _uiFactory;

        public void Initialize(PotionOrdersHandler ordersHandler, IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _ordersHandler = ordersHandler;
            
            _ordersHandler.NewOrderHandled += UpdateOrderDifficultyLevelAndTypeNames;
            _ordersHandler.NewOrderHandled += UpdateRequirementCharacteristics;
            _ordersHandler.NewOrderHandled += UpdateReward;
            
            UpdateOrderDifficultyLevelAndTypeNames();
            UpdateRequirementCharacteristics();
            UpdateReward();
        }

        private void OnDestroy()
        {
            _ordersHandler.NewOrderHandled -= UpdateOrderDifficultyLevelAndTypeNames;
            _ordersHandler.NewOrderHandled -= UpdateRequirementCharacteristics;
            _ordersHandler.NewOrderHandled -= UpdateReward;

        }

        private void UpdateOrderDifficultyLevelAndTypeNames()
        {
            _orderDifficultyLevelNameText.text = _ordersHandler.CurrentOrder.OrderDifficultyName.ToUpper();
            _orderTypeNameText.text = _ordersHandler.CurrentOrder.OrderTypeName.ToUpper();
        }

        private void UpdateRequirementCharacteristics()
        {
            StartCoroutine(UpdateRequirementCharacteristicsCoroutine());
        }

        private void UpdateReward()
        {
            _rewardItem.SetReward(_ordersHandler.CurrentOrder.Reward);
        }

        private IEnumerator UpdateRequirementCharacteristicsCoroutine()
        {
            Task task = _requirementCharacteristicsContainer.CreateCharacteristicItemsAsync(
                _ordersHandler.CurrentOrder.RequirementCharacteristics,
                _uiFactory);

            yield return task;
        }
    }
}