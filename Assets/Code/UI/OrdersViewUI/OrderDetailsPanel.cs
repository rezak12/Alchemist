using System;
using Code.Infrastructure.Services.Factories;
using Code.Logic.Orders;
using Code.UI.PotionCharacteristicsUI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

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
        
        private Action _updateRequirementCharacteristicsAction;
        
        public void Initialize(PotionOrdersHandler ordersHandler)
        {
            _ordersHandler = ordersHandler;
            _updateRequirementCharacteristicsAction = UniTask.Action(UpdateRequirementCharacteristics);
            
            _ordersHandler.NewOrderHandled += UpdateOrderDifficultyLevelAndTypeNames;
            _ordersHandler.NewOrderHandled += _updateRequirementCharacteristicsAction;
            _ordersHandler.NewOrderHandled += UpdateReward;
            _ordersHandler.NewOrderHandled += UpdatePunishment;
            
            UpdateOrderDifficultyLevelAndTypeNames();
            UpdateRequirementCharacteristics().Forget();
            UpdateReward();
            UpdatePunishment();
        }

        private void OnDestroy()
        {
            _ordersHandler.NewOrderHandled -= UpdateOrderDifficultyLevelAndTypeNames;
            _ordersHandler.NewOrderHandled -= _updateRequirementCharacteristicsAction;
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
            _rewardItem.SetReward(_ordersHandler.CurrentOrder.Reward).Forget();
        }

        private void UpdatePunishment()
        {
            _punishmentItem.SetPunishment(_ordersHandler.CurrentOrder.Punishment);
        }

        private async UniTaskVoid UpdateRequirementCharacteristics()
        {
            await _requirementCharacteristicsContainer.CreateCharacteristicItemsAsync(
                _ordersHandler.CurrentOrder.RequirementCharacteristics);
        }
    }
}