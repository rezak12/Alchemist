using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Logic.Potions
{
    public class Potion : MonoBehaviour
    {
        public IEnumerable<PotionCharacteristicAmountPair> CharacteristicAmountPairs =>
            _potionInfo.CharacteristicsAmountPairs;
        public AssetReferenceT<AudioClip> SFXReference { get; private set; }
        
        private PotionInfo _potionInfo;

        public void Initialize(PotionInfo potionInfo, AssetReferenceT<AudioClip> sfxReference)
        {
            _potionInfo = potionInfo;
            SFXReference = sfxReference;
        }
    }
}