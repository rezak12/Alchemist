using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Infrastructure.GameStates;
using Code.Infrastructure.Services.Factories;
using Code.Logic.Orders;
using Code.Logic.Potions;
using Code.UI.OrdersViewUI;
using Code.UI.PotionCharacteristicsUI;
using UnityEngine;
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
        
        private GameStateMachine _gameStateMachine;
        private IUIFactory _uiFactory;

        [Inject]
        private void Construct(GameStateMachine stateMachine, IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _gameStateMachine = stateMachine;
        }

        public async Task InitializeAsync(
            List<PotionCharacteristicAmountPair> resultCharacteristics,
            List<PotionCharacteristicAmountPair> requirementCharacteristics,
            PotionOrderReward reward)
        {
            Task fillCharacteristicsTask = FillCharacteristicItemsContainers(
                resultCharacteristics, requirementCharacteristics);
            
            InitializeOpenMenuButton();
            FillRewardItem(reward);

            await fillCharacteristicsTask;
        }

        public async Task InitializeAsync(
            List<PotionCharacteristicAmountPair> resultCharacteristics,
            List<PotionCharacteristicAmountPair> requirementCharacteristics,
            PotionOrderPunishment punishment
            )
        {
            Task fillCharacteristicsTask = FillCharacteristicItemsContainers(
                resultCharacteristics, requirementCharacteristics);
            
            InitializeOpenMenuButton();
            FillPunishmentItem(punishment);

            await fillCharacteristicsTask;
        }

        private void OnDestroy()
        {
            _openMenuButton.onClick.RemoveListener(OpenMenu);
        }

        private async Task FillCharacteristicItemsContainers(
            List<PotionCharacteristicAmountPair> resultCharacteristics, 
            List<PotionCharacteristicAmountPair> requirementCharacteristics)
        {
            Task resultCharacteristicItemsTask = _resultCharacteristics
                .CreateCharacteristicItemsAsync(resultCharacteristics, _uiFactory);

            Task requirementCharacteristicItemsTask = _requirementCharacteristics
                .CreateCharacteristicItemsAsync(requirementCharacteristics, _uiFactory);

            await Task.WhenAll(resultCharacteristicItemsTask, requirementCharacteristicItemsTask);
        }

        private void FillRewardItem(PotionOrderReward reward)
        {
            _rewardItem.SetReward(reward);
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
            _openMenuButton.onClick.AddListener(OpenMenu);
        }

        private void OpenMenu()
        {
            //switch gameStateMachineState
        }
    }
}