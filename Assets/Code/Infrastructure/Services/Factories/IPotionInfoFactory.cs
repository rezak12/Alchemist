using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Logic.Potions;
using Code.StaticData;

namespace Code.Infrastructure.Services.Factories
{
    public interface IPotionInfoFactory
    {
        Task<PotionInfo> CreatePotionInfoAsync(IEnumerable<IngredientData> ingredients);
    }
}