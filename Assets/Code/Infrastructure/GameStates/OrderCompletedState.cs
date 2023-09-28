using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.SaveLoadService;
using Code.Logic.Orders;
using Code.Logic.PotionMaking;
using Code.Logic.Potions;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.GameStates
{
    public class OrderCompletedState : IPayloadState<Potion, PotionOrder>
    {
        private readonly ResultPotionRater _potionRater;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IUIFactory _uiFactory;
        
        private UniTaskCompletionSource _taskCompletionSource;

        public OrderCompletedState(
            ResultPotionRater potionRater, 
            IPersistentProgressService progressService, 
            ISaveLoadService saveLoadService,
            IUIFactory uiFactory)
        {
            _potionRater = potionRater;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _uiFactory = uiFactory;
        }

        public UniTask Enter(Potion payload1, PotionOrder payload2)
        {
            _taskCompletionSource = new UniTaskCompletionSource();
            
            var isRequirementsMatched = _potionRater.IsPotionSatisfyingRequirements(payload1, payload2);

            if (isRequirementsMatched)
            {
                GiveReward(payload2.Reward);
            }
            else
            {
                GivePunishment(payload2.Punishment);
            }

            SaveProgress();
            CreateUIWindow(payload1, payload2, isRequirementsMatched);

            return _taskCompletionSource.Task;
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