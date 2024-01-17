using System;
using System.Collections.Generic;

namespace Code.Infrastructure.Services.ProgressServices
{
    [Serializable]
    public class PlayerProgress
    {
        public int CoinsAmount;
        public int ReputationAmount;
        public List<string> IngredientsGuids;
        public string PotionDataGuid;
        public string AlchemyTablePrefabGuid;
        public string EnvironmentPrefabGuid;
        public PlayerOwnedItems PlayerItems;

        public PlayerProgress(
            int coinsAmount, 
            int reputationAmount, 
            IEnumerable<string> ingredientsGuids, 
            string potionDataGuid, 
            string alchemyTablePrefabGuid, 
            string environmentPrefabGuid, 
            PlayerOwnedItems playerItems)
        {
            CoinsAmount = coinsAmount;
            ReputationAmount = reputationAmount;
            IngredientsGuids = new List<string>(ingredientsGuids);
            PotionDataGuid = potionDataGuid;
            AlchemyTablePrefabGuid = alchemyTablePrefabGuid;
            EnvironmentPrefabGuid = environmentPrefabGuid;
            PlayerItems = playerItems;
        }
    }

    [Serializable]
    public class PlayerOwnedItems
    {
        public List<string> PotionGuids;
        public List<string> TableGuids;
        public List<string> EnvironmentGuids;

        public PlayerOwnedItems(List<string> potionGuids, List<string> tableGuids, List<string> environmentGuids)
        {
            PotionGuids = potionGuids;
            TableGuids = tableGuids;
            EnvironmentGuids = environmentGuids;
        }
    }
}