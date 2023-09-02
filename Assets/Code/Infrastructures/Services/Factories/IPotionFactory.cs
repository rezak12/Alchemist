using System.Collections.Generic;
using Code.Logic.Potions;
using Code.StaticData;

namespace Code.Infrastructures.Services.Factories
{
    public interface IPotionFactory
    {
        PotionInfo CreatePotion(IEnumerable<IngredientData> ingredients);
    }
}