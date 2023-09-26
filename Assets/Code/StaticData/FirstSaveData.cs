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

        public int CoinsAmount => _coinsAmount;
        public int ReputationAmount => _reputationAmount;
        public AssetReferenceT<IngredientData>[] Ingredients => _ingredients;
        public AssetReferenceT<Potion> PotionPrefabReference => _potionPrefabReference;
    }
}