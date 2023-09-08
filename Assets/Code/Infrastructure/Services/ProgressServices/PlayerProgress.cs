using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Code.Infrastructure.Services.ProgressServices
{
    [Serializable]
    public class PlayerProgress
    {
        public int CoinsAmount;
        public int ReputationAmount;
        public List<string> PlayerIngredientsGUIDs;
        public string PlayerPotionPrefabGUID;

        public PlayerProgress(
            int coinsAmount, 
            int reputationAmount, 
            IEnumerable<string> playerIngredientsGUIDs, 
            string playerPotionPrefabGuid)
        {
            CoinsAmount = coinsAmount;
            ReputationAmount = reputationAmount;
            PlayerIngredientsGUIDs = new List<string>(playerIngredientsGUIDs);
            PlayerPotionPrefabGUID = playerPotionPrefabGuid;
        }
    }
}