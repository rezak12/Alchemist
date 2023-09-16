using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.SaveLoadService;
using Code.Logic.Orders;
using Code.Logic.PotionMaking;
using Code.Logic.Potions;

namespace Code.Infrastructure.GameStates
{
    public class OrderCompletedState : IPayloadState<PotionOrder, Potion>
    {
        private readonly ResultPotionRater _potionRater;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IUIFactory _uiFactory;

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

        public void Enter(PotionOrder payload1, Potion payload2)
        {
            var isRequirementsMatched = _potionRater.RatePotionByOrderRequirementCharacteristics(payload1, payload2);

            if (isRequirementsMatched)
            {
                GiveReward(payload1.Reward);
            }
            else
            {
                GivePunishment(payload1.Punishment);
            }

            SaveProgress();
            CreateUIWindow();
        }

        public void Exit()
        {
            
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

        private void CreateUIWindow()
        {
            _uiFactory.CreateOrderCompletedPopupAsync();
        }
    }
}