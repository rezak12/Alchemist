using System.Collections.Generic;
using Code.Animations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "IngredientData", menuName = "StaticData/Ingredient")]
    public class IngredientData : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField] private AssetReferenceGameObject _prefabReference;
        [FormerlySerializedAs("_characteristics")] 
        [SerializeField] private List<IngredientCharacteristicAmountPair> _characteristicAmountPairs;

        public Sprite Icon => _icon;
        public string Name => _name;
        public AssetReferenceGameObject PrefabReference => _prefabReference;
        public List<IngredientCharacteristicAmountPair> CharacteristicAmountPairs => _characteristicAmountPairs;
    }
}