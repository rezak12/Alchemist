﻿using System.Collections.Generic;
using System.Linq;
using Code.StaticData.Ingredients;
using Code.StaticData.Potions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.StaticData.Configs
{
    [CreateAssetMenu(fileName = "FirstSave", menuName = "StaticData/FirstSaveData")]
    public class FirstSaveData : ScriptableObject
    {
        [SerializeField] private int _coinsAmount;
        [SerializeField] private int _reputationAmount;
        [SerializeField] private AssetReferenceT<IngredientData>[] _ingredients;
        [SerializeField] private AssetReferenceT<PotionData> _potionDataReference;
        [SerializeField] private AssetReferenceGameObject _alchemyTablePrefabReference;
        [SerializeField] private AssetReferenceGameObject _environmentReference;

        public int CoinsAmount => _coinsAmount;
        public int ReputationAmount => _reputationAmount;
        public IEnumerable<string> IngredientsGUIDs => _ingredients.Select(reference => reference.AssetGUID);
        public string PotionDataGUID => _potionDataReference.AssetGUID;
        public string AlchemyTablePrefabGUID => _alchemyTablePrefabReference.AssetGUID;
        public string EnvironmentPrefabGUID => _environmentReference.AssetGUID;
    }
}