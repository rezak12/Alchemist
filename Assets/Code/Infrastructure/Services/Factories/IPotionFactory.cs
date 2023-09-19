using System.Threading.Tasks;
using Code.Logic.Potions;
using UnityEngine;

namespace Code.Infrastructure.Services.Factories
{
    public interface IPotionFactory
    {
        public Task<Potion> CreatePotionAsync(PotionInfo potionInfo, Vector3 position);
    }
}