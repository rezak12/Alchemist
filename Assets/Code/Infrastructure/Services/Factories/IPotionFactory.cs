using Code.Logic.Potions;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.Factories
{
    public interface IPotionFactory
    {
        public UniTask<Potion> CreatePotionAsync(PotionInfo potionInfo, Vector3 position);
    }
}