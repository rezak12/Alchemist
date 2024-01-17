using System;
using Code.StaticData.Potions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.StaticData.Ingredients
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