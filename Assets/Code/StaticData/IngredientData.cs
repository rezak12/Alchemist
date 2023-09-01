using System.Collections.Generic;
using System.Linq;
using Code.Logic.Potions;
using UnityEngine;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "IngredientData", menuName = "StaticData/Ingredient")]
    public class IngredientData : ScriptableObject
    {
        [SerializeField] private List<IngredientCharacteristicAmountPair> _characteristics;

        private List<PotionCharacteristicAmountPair> _convertedCharacteristics;
        public List<PotionCharacteristicAmountPair> Characteristics
        {
            get
            {
                if (_convertedCharacteristics == null)
                {
                    ConvertInputToCharacteristicAmountPairList();
                }

                return _convertedCharacteristics;
            }
        }

        private void ConvertInputToCharacteristicAmountPairList()
        {
            _convertedCharacteristics = _characteristics
                .Select(characteristic =>
                new PotionCharacteristicAmountPair(characteristic.Characteristic, characteristic.Amount))
                .ToList();
        }
    }
}