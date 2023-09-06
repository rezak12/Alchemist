using System.Collections.Generic;
using UnityEngine;

namespace Code.Logic.Potions
{
    public class Potion : MonoBehaviour
    {
        public IEnumerable<PotionCharacteristicAmountPair> CharacteristicAmountPairs =>
            _potionInfo.CharacteristicsAmountPairs;
        
        private PotionInfo _potionInfo;

        public void Initialize(PotionInfo potionInfo)
        {
            _potionInfo = potionInfo;
        }
    }
}