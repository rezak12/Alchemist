using System;
using System.Collections.Generic;
using System.Linq;
using Code.StaticData;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.ProgressServices
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public int CoinsAmount => _playerProgress.CoinsAmount;
        public int ReputationAmount => _playerProgress.ReputationAmount;
        public List<AssetReferenceT<IngredientData>> PlayerIngredientsAssetReferences { get; private set; }
        public AssetReferenceT<PotionData> ChosenPotionDataReference { get; private set; }
        public AssetReferenceGameObject ChosenAlchemyTablePrefabReference { get; private set; }

        public event Action CoinsAmountChanged;
        public event Action ReputationAmountChanged;
        
        private PlayerProgress _playerProgress;

        public void Initialize(PlayerProgress progress)
        {
            _playerProgress = progress;
            
            PlayerIngredientsAssetReferences = progress
                .PlayerIngredientsGUIDs
                .Select(guid => new AssetReferenceT<IngredientData>(guid))
                .ToList();

            ChosenPotionDataReference = new AssetReferenceT<PotionData>(progress.PlayerPotionDataGUID);
            ChosenAlchemyTablePrefabReference = new AssetReferenceGameObject(progress.PlayerAlchemyTablePrefabGUID);
        }

        public PlayerProgress GetProgress() => _playerProgress;

        public void AddCoins(int amount)
        {
            if (amount < 0)
            {
                Debug.LogError("Incorrect coins amount transferred!");
                return;
            }
            
            _playerProgress.CoinsAmount += amount;
            CoinsAmountChanged?.Invoke();
        }

        public void AddReputation(int amount)
        {
            if (amount < 0)
            {
                Debug.LogError("Incorrect reputation amount transferred!");
                return;
            }
            
            _playerProgress.ReputationAmount += amount;
            ReputationAmountChanged?.Invoke();
        }

        public void AddNewIngredient(AssetReferenceT<IngredientData> ingredientReference)
        {
            if (_playerProgress.PlayerIngredientsGUIDs.Contains(ingredientReference.AssetGUID))
            {
                return;
            }
            
            _playerProgress.PlayerIngredientsGUIDs.Add(ingredientReference.AssetGUID);
            PlayerIngredientsAssetReferences.Add(ingredientReference);
        }

        public void RemoveCoins(int amount)
        {
            if (!IsCoinsEnoughFor(amount))
            {
                Debug.LogError("Incorrect coins amount transferred!");
            }

            _playerProgress.CoinsAmount -= amount;
            CoinsAmountChanged?.Invoke();
        }

        public void RemoveReputation(int amount)
        {
            _playerProgress.ReputationAmount -= amount;
            ReputationAmountChanged?.Invoke();
        }

        public bool IsCoinsEnoughFor(int itemPrice) => CoinsAmount >= itemPrice;

        public bool IsReputationEnoughFor(int itemPrice) => ReputationAmount >= itemPrice;
    }
}