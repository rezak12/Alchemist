using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "IngredientData", menuName = "StaticData/Ingredient")]
    public class IngredientData : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField] private AssetReferenceGameObject _prefabReference;
        [SerializeField] private AssetReferenceT<AudioClip> _audioClipReference;
        [SerializeField] private List<IngredientCharacteristicAmountPair> _characteristicAmountPairs;

        public Sprite Icon => _icon;
        public string Name => _name;
        public AssetReferenceGameObject PrefabReference => _prefabReference;
        public AssetReferenceT<AudioClip> AudioClipReference => _audioClipReference;
        public List<IngredientCharacteristicAmountPair> CharacteristicAmountPairs => _characteristicAmountPairs;
    }
}