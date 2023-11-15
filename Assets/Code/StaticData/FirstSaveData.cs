using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "FirstSave", menuName = "StaticData/FirstSaveData")]
    public class FirstSaveData : ScriptableObject
    {
        [SerializeField] private int _coinsAmount;
        [SerializeField] private int _reputationAmount;
        [SerializeField] private AssetReferenceT<IngredientData>[] _ingredients;
        [SerializeField] private AssetReferenceT<PotionData> _potionDataReference;
        [SerializeField] private AssetReferenceGameObject _alchemyTablePrefabReference;

        public int CoinsAmount => _coinsAmount;
        public int ReputationAmount => _reputationAmount;
        public IEnumerable<string> IngredientsGUIDs => _ingredients.Select(reference => reference.AssetGUID);
        public string PotionDataGUID => _potionDataReference.AssetGUID;
        public string AlchemyTablePrefabGUID => _alchemyTablePrefabReference.AssetGUID;
    }
}