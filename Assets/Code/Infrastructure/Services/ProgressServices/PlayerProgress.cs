using System;
using System.Collections.Generic;

namespace Code.Infrastructure.Services.ProgressServices
{
    [Serializable]
    public class PlayerProgress
    {
        public int CoinsAmount;
        public int ReputationAmount;
        public List<string> PlayerIngredientsGUIDs;
        public string PlayerPotionDataGUID;
        public string PlayerAlchemyTablePrefabGUID;
        public string PlayerEnvironmentPrefabGUID;

        public PlayerProgress(
            int coinsAmount, 
            int reputationAmount, 
            IEnumerable<string> playerIngredientsGUIDs, 
            string playerPotionDataGuid, 
            string playerAlchemyTablePrefabGuid, 
            string playerEnvironmentPrefabGuid)
        {
            CoinsAmount = coinsAmount;
            ReputationAmount = reputationAmount;
            PlayerIngredientsGUIDs = new List<string>(playerIngredientsGUIDs);
            PlayerPotionDataGUID = playerPotionDataGuid;
            PlayerAlchemyTablePrefabGUID = playerAlchemyTablePrefabGuid;
            PlayerEnvironmentPrefabGUID = playerEnvironmentPrefabGuid;
        }
    }
}