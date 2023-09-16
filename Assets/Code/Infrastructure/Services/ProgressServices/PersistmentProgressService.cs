using System;
using System.Collections.Generic;
using System.Linq;
using Code.Logic.Potions;
using Code.StaticData;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.ProgressServices
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public int CoinsAmount => _playerProgress.CoinsAmount;
        public int ReputationAmount => _playerProgress.ReputationAmount;
        public HashSet<AssetReferenceT<IngredientData>> PlayerIngredientsAssetReferences { get; private set; }
        public AssetReferenceT<Potion> CurrentPlayerPotionPrefabReference { get; private set; }

        public event Action CoinsAmountChanged;
        public event Action ReputationAmountChanged;
        
        private PlayerProgress _playerProgress;

        public void Initialize(PlayerProgress progress)
        {
            _playerProgress = progress;
            
            PlayerIngredientsAssetReferences = progress
                .PlayerIngredientsGUIDs
                .Select(guid => new AssetReferenceT<IngredientData>(guid))
                .ToHashSet();

            CurrentPlayerPotionPrefabReference = new AssetReferenceT<Potion>(progress.PlayerPotionPrefabGUID);
        }

        public PlayerProgress GetProgress()
        {
            return _playerProgress;
        }
        
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
            PlayerIngredientsAssetReferences.Add(ingredientReference);
            _playerProgress.PlayerIngredientsGUIDs.Add(ingredientReference.AssetGUID);
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
            if (!IsReputationEnoughFor(amount))
            {
                Debug.LogError("Incorrect reputation amount transferred!");
            }

            _playerProgress.ReputationAmount -= amount;
            ReputationAmountChanged?.Invoke();
        }

        public bool IsCoinsEnoughFor(int itemPrice)
        {
            return CoinsAmount >= itemPrice;
        }
        
        public bool IsReputationEnoughFor(int itemPrice)
        {
            return ReputationAmount >= itemPrice;
        }
    }
}