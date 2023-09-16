using Code.StaticData;
using UnityEngine.AddressableAssets;

namespace Code.Logic.Orders
{
    public class PotionOrderReward
    {
        public int CoinsAmount { get; }
        public int ReputationAmount { get; }
        public AssetReferenceT<IngredientData> IngredientReference { get; }

        public PotionOrderReward(
            int coinsAmount, 
            int reputationAmount, 
            AssetReferenceT<IngredientData> ingredientReference)
        {
            CoinsAmount = coinsAmount;
            ReputationAmount = reputationAmount;
            IngredientReference = ingredientReference;
        }
    }
}