using Code.StaticData;

namespace Code.Logic.Orders
{
    public class OrderReward
    {
        public int CoinsAmount { get; }
        public int ReputationAmount { get; }
        public IngredientData Ingredient { get; }

        public OrderReward(int coinsAmount, int reputationAmount, IngredientData ingredient)
        {
            CoinsAmount = coinsAmount;
            ReputationAmount = reputationAmount;
            Ingredient = ingredient;
        }
    }
}