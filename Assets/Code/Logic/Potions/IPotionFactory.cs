using System.Collections.Generic;
using Code.StaticData;

namespace Code.Logic.Potions
{
    public interface IPotionFactory
    {
        PotionInfo CreatePotion(IEnumerable<IngredientData> ingredients);
    }
}