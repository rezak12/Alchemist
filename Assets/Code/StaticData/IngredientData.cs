using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "IngredientData", menuName = "StaticData/Ingredient")]
    public class IngredientData : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [FormerlySerializedAs("_characteristics")] 
        [SerializeField] private List<IngredientCharacteristicAmountPair> _characteristicAmountPairs;

        public Sprite Icon => _icon;
        public string Name => _name;
        public IEnumerable<IngredientCharacteristicAmountPair> CharacteristicAmountPairs => _characteristicAmountPairs;
    }
}