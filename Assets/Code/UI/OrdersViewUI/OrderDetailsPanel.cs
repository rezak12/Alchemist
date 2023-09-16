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
        [SerializeField] private PotionOrderPunishmentItemUi _punishmentItem;
        
        private PotionOrdersHandler _ordersHandler;
        private IUIFactory _uiFactory;

        public void Initialize(PotionOrdersHandler ordersHandler, IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _ordersHandler = ordersHandler;
            
            _ordersHandler.NewOrderHandled += UpdateOrderDifficultyLevelAndTypeNames;
            _ordersHandler.NewOrderHandled += UpdateRequirementCharacteristics;
            _ordersHandler.NewOrderHandled += UpdateReward;
            _ordersHandler.NewOrderHandled += UpdatePunishment;
            
            UpdateOrderDifficultyLevelAndTypeNames();
            UpdateRequirementCharacteristics();
            UpdateReward();
            UpdatePunishment();
        }

        private void OnDestroy()
        {
            _ordersHandler.NewOrderHandled -= UpdateOrderDifficultyLevelAndTypeNames;
            _ordersHandler.NewOrderHandled -= UpdateRequirementCharacteristics;
            _ordersHandler.NewOrderHandled -= UpdateReward;
            _ordersHandler.NewOrderHandled -= UpdatePunishment;
        }

        private void UpdateOrderDifficultyLevelAndTypeNames()
        {
            _orderDifficultyLevelNameText.text = _ordersHandler.CurrentOrder.OrderDifficultyName.ToUpper();
            _orderTypeNameText.text = _ordersHandler.CurrentOrder.OrderTypeName.ToUpper();
        }

        private void UpdateReward()
        {
            _rewardItem.SetReward(_ordersHandler.CurrentOrder.Reward);
        }

        private void UpdatePunishment()
        {
            _punishmentItem.SetPunishment(_ordersHandler.CurrentOrder.Punishment);
        }

        private void UpdateRequirementCharacteristics()
        {
            StartCoroutine(UpdateRequirementCharacteristicsCoroutine());
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