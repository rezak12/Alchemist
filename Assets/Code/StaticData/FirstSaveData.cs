using System.Collections.Generic;
using System.Linq;
using Code.Logic.PotionMaking;
using Code.Logic.Potions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "FirstSave", menuName = "StaticData/FirstSaveData")]
    public class FirstSaveData : ScriptableObject
    {
        [SerializeField] private int _coinsAmount;
        [SerializeField] private int _reputationAmount;
        [SerializeField] private AssetReferenceT<IngredientData>[] _ingredients;
        [SerializeField] private AssetReferenceGameObject _potionPrefabReference;
        [SerializeField] private AssetReferenceGameObject _alchemyTablePrefabReference;

        public int CoinsAmount => _coinsAmount;
        public int ReputationAmount => _reputationAmount;
        public IEnumerable<string> IngredientsGUIDs => _ingredients.Select(reference => reference.AssetGUID);
        public string PotionPrefabGUID => _potionPrefabReference.AssetGUID;
        public string AlchemyTablePrefabGUID => _alchemyTablePrefabReference.AssetGUID;
    }
}