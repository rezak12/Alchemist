using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Logic.Potions;
using Code.StaticData;
using UnityEngine;

namespace Code.Infrastructure.Services.Factories
{
    public interface IPotionFactory
    {
        Task<Potion> CreatePotionAsync(IEnumerable<IngredientData> ingredients, Vector3 position);
    }
}