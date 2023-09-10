using Code.StaticData;

namespace Code.Logic.Orders
{
    public class PotionOrderReward
    {
        public int CoinsAmount { get; }
        public int ReputationAmount { get; }
        public IngredientData Ingredient { get; }

        public PotionOrderReward(int coinsAmount, int reputationAmount, IngredientData ingredient)
        {
            CoinsAmount = coinsAmount;
            ReputationAmount = reputationAmount;
            Ingredient = ingredient;
        }
    }
}