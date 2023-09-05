using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.StaticData
{
    [Serializable]
    public class IngredientCharacteristicAmountPair
    {
        [SerializeField] private AssetReferenceT<PotionCharacteristic> _characteristic;
        [SerializeField] private int _amount;
        
        public AssetReferenceT<PotionCharacteristic> CharacteristicReference => _characteristic;
        public int PointsAmount => _amount;
    }
}