using System;
using UnityEngine;

namespace Code.StaticData
{
    [Serializable]
    public class IngredientCharacteristicAmountPair
    {
        [SerializeField] private PotionCharacteristic _characteristic;
        [SerializeField] private int _amount;
        
        public PotionCharacteristic Characteristic => _characteristic;
        public int Amount => _amount;
    }
}