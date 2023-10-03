using System.Collections.Generic;
using System.Linq;
using Code.Logic.PotionMaking;
using Code.Logic.Potions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "firstSave", menuName = "StaticData/FirstSaveData")]
    public class FirstSaveData : ScriptableObject
    {
        [SerializeField] private int _coinsAmount;
        [SerializeField] private int _reputationAmount;
        [SerializeField] private AssetReferenceT<IngredientData>[] _ingredients;
        [SerializeField] private AssetReferenceT<Potion> _potionPrefabReference;
        [SerializeField] private AssetReferenceT<AlchemyTable> _alchemyTablePrefabReference;

        public int CoinsAmount => _coinsAmount;
        public int ReputationAmount => _reputationAmount;
        public IEnumerable<string> IngredientsGUIDs => _ingredients.Select(reference => reference.AssetGUID);
        public string PotionPrefabGUID => _potionPrefabReference.AssetGUID;
        public string AlchemyTablePrefabGUID => _alchemyTablePrefabReference.AssetGUID;
    }
}