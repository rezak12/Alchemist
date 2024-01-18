using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.SaveLoadService;
using Code.Logic.Orders;
using Code.Logic.PotionMaking;
using Code.Logic.Potions;
using Code.UI.AwaitingOverlays;
using Code.UI.OrderCompletedUI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.States.PotionMakingStates
{
    public class OrderCompletedState : IPayloadState<Potion>
    {
        private readonly ResultPotionRater _potionRater;
        private readonly SelectedPotionOrderHolder _orderHolder;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IUIFactory _uiFactory;
        private readonly IAwaitingOverlay _awaitingOverlay;
        
        private OrderCompletedPopup _orderCompletedPopup;

        public OrderCompletedState(
            SelectedPotionOrderHolder orderHolder, 
            IPersistentProgressService progressService, 
            ISaveLoadService saveLoadService,
            IUIFactory uiFactory,
            IAwaitingOverlay awaitingOverlay)
        {
            _potionRater = new ResultPotionRater();
            _orderHolder = orderHolder;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _uiFactory = uiFactory;
            _awaitingOverlay = awaitingOverlay;
        }

        public async UniTask Enter(Potion payload1)
        { 
            PotionOrder order = _orderHolder.SelectedOrder;
            var isRequirementsMatched = _potionRater.IsPotionSatisfyingRequirements(payload1, order);

            if (isRequirementsMatched)
            {
                GiveReward(order.Reward);
            }
            else
            {
                GivePunishment(order.Punishment);
            }
            await SaveProgress();
            
            await CreateUIWindow(payload1, order, isRequirementsMatched);
            
            Object.Destroy(payload1.gameObject);
        }

        public async UniTask Exit()
        {
            await _awaitingOverlay.Show();
            Object.Destroy(_orderCompletedPopup.gameObject);
        }

        private void GiveReward(PotionOrderReward reward)
        {
            _progressService.AddCoins(reward.CoinsAmount);
            _progressService.AddReputation(reward.ReputationAmount);
            if (reward.IngredientReference != null)
            {
                _progressService.AddNewIngredient(reward.IngredientReference);
            }
        }

        private void GivePunishment(PotionOrderPunishment punishment) => 
            _progressService.RemoveReputation(punishment.ReputationAmount);

        private async UniTask SaveProgress() => 
            await _saveLoadService.SaveProgress(_progressService.GetProgress());

        private async UniTask CreateUIWindow(Potion resultPotion, PotionOrder order, bool isCharacteristicsMatched) =>
            _orderCompletedPopup = await _uiFactory.CreateOrderCompletedPopupAsync(
                resultPotion, 
                order, 
                isCharacteristicsMatched);
    }
}