using System.Collections.Generic;
using Code.Infrastructure.States.PotionMakingStates;
using Code.Logic.Orders;
using Code.Logic.Potions;
using Code.UI.PotionCharacteristicsUI;
using Code.UI.SelectionPotionOrderUI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.OrderCompletedUI
{
    public class OrderCompletedPopup : MonoBehaviour
    {
        [SerializeField] private PotionCharacteristicItemsContainer _resultCharacteristics;
        [SerializeField] private PotionCharacteristicItemsContainer _requirementCharacteristics;

        [SerializeField] private PotionOrderRewardItemUI _rewardItem;
        [SerializeField] private PotionOrderPunishmentItemUi _punishmentItem;
        
        [SerializeField] private Button _openMenuButton;
        
        private PotionMakingLevelStateMachine _gameStateMachine;
        private UnityAction _openMenuAction;

        [Inject]
        private void Construct(PotionMakingLevelStateMachine stateMachine)
        {
            _gameStateMachine = stateMachine;
        }

        public async UniTask InitializeAsync(
            IEnumerable<PotionCharacteristicAmountPair> resultCharacteristics,
            IEnumerable<PotionCharacteristicAmountPair> requirementCharacteristics,
            PotionOrderReward reward)
        {
            UniTask fillCharacteristicsTask = FillCharacteristicItemsContainers(
                resultCharacteristics, requirementCharacteristics);
            
            InitializeOpenMenuButton();
            FillRewardItem(reward);

            await fillCharacteristicsTask;
        }

        public async UniTask InitializeAsync(
            IEnumerable<PotionCharacteristicAmountPair> resultCharacteristics,
            IEnumerable<PotionCharacteristicAmountPair> requirementCharacteristics,
            PotionOrderPunishment punishment
            )
        {
            UniTask fillCharacteristicsTask = FillCharacteristicItemsContainers(
                resultCharacteristics, requirementCharacteristics);
            
            InitializeOpenMenuButton();
            FillPunishmentItem(punishment);

            await fillCharacteristicsTask;
        }

        private void OnDestroy()
        {
            _openMenuButton.onClick.RemoveListener(_openMenuAction);
        }

        private async UniTask FillCharacteristicItemsContainers(
            IEnumerable<PotionCharacteristicAmountPair> resultCharacteristics, 
            IEnumerable<PotionCharacteristicAmountPair> requirementCharacteristics)
        {
            UniTask resultCharacteristicItemsTask = _resultCharacteristics
                .CreateCharacteristicItemsAsync(resultCharacteristics);

            UniTask requirementCharacteristicItemsTask = _requirementCharacteristics
                .CreateCharacteristicItemsAsync(requirementCharacteristics);

            await UniTask.WhenAll(resultCharacteristicItemsTask, requirementCharacteristicItemsTask);
        }

        private void FillRewardItem(PotionOrderReward reward)
        {
            _rewardItem.SetReward(reward).Forget();
            _rewardItem.gameObject.SetActive(true);
            _punishmentItem.gameObject.SetActive(false);
        }

        private void FillPunishmentItem(PotionOrderPunishment punishment)
        {
            _punishmentItem.SetPunishment(punishment);
            _punishmentItem.gameObject.SetActive(true);
            _rewardItem.gameObject.SetActive(false);
        }

        private void InitializeOpenMenuButton()
        {
            _openMenuAction = UniTask.UnityAction(OpenOrderSelectionState);
            _openMenuButton.onClick.AddListener(_openMenuAction);
        }

        private async UniTaskVoid OpenOrderSelectionState()
        {
            await _gameStateMachine.Enter<OrderSelectionState>();
        }
    }
}