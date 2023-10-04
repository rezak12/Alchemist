using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.SaveLoadService;
using Code.Logic.Orders;
using Code.Logic.PotionMaking;
using Code.Logic.Potions;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States.PotionMakingStates
{
    public class OrderCompletedState : IPayloadState<Potion>
    {
        private readonly ResultPotionRater _potionRater;
        private readonly SelectedPotionOrderHolder _orderHolder;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IUIFactory _uiFactory;

        public OrderCompletedState(
            SelectedPotionOrderHolder orderHolder, 
            IPersistentProgressService progressService, 
            ISaveLoadService saveLoadService,
            IUIFactory uiFactory)
        {
            _potionRater = new ResultPotionRater();
            _orderHolder = orderHolder;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _uiFactory = uiFactory;
        }

        public async UniTask Enter(Potion payload1)
        {
            await UniTask.Yield();

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

            SaveProgress();
            CreateUIWindow(payload1, order, isRequirementsMatched);
        }

        public UniTask Exit()
        {
            return default;
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

        private void GivePunishment(PotionOrderPunishment punishment)
        {
            _progressService.RemoveReputation(punishment.ReputationAmount);
        }

        private void SaveProgress()
        {
            _saveLoadService.SaveProgress(_progressService.GetProgress());
        }

        private void CreateUIWindow(Potion resultPotion, PotionOrder order, bool isCharacteristicsMatched)
        {
            _uiFactory.CreateOrderCompletedPopupAsync(resultPotion, order, isCharacteristicsMatched);
        }
    }
}