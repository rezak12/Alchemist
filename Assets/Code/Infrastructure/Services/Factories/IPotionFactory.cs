using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Logic.Potions;
using Code.StaticData;

namespace Code.Infrastructure.Services.Factories
{
    public interface IPotionFactory
    {
        Task<PotionInfo> CreatePotionAsync(IEnumerable<IngredientData> ingredients);
    }
}