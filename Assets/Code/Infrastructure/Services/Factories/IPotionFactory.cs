using System.Collections.Generic;
using Code.Logic.Potions;
using Code.StaticData;

namespace Code.Infrastructure.Services.Factories
{
    public interface IPotionFactory
    {
        PotionInfo CreatePotion(IEnumerable<IngredientData> ingredients);
    }
}