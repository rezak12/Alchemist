using System.Collections.Generic;
using UnityEngine;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "IngredientData", menuName = "StaticData/Ingredient")]
    public class IngredientData : ScriptableObject
    {
        [SerializeField] private List<IngredientCharacteristicAmountPair> _characteristics;
        public IEnumerable<IngredientCharacteristicAmountPair> Characteristics => _characteristics;
    }
}