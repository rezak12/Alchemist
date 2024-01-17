using System;
using System.Collections.Generic;
using System.Linq;
using Code.StaticData.Ingredients;
using Code.StaticData.Potions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.ProgressServices
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public int CoinsAmount => _playerProgress.CoinsAmount;
        public int ReputationAmount => _playerProgress.ReputationAmount;

        public IEnumerable<AssetReferenceT<IngredientData>> OwnedIngredientsAssetReferences =>
            _ownedIngredientsAssetReferences;
        public AssetReferenceT<PotionData> ChosenPotionDataReference { get; private set; }
        public AssetReferenceGameObject ChosenAlchemyTablePrefabReference { get; private set; }
        public AssetReferenceGameObject ChosenEnvironmentPrefabReference { get; private set; }

        public IReadOnlyCollection<AssetReferenceT<PotionData>> OwnedPotionsReferences => _ownedPotionsReferences;
        public IReadOnlyCollection<AssetReferenceGameObject> OwnedTablesReferences => _ownedTablesReferences;
        public IReadOnlyCollection<AssetReferenceGameObject> OwnedEnvironmentsReferences => _ownedEnvironmentReferences;

        public event Action CoinsAmountChanged;
        public event Action ReputationAmountChanged;
        
        private List<AssetReferenceT<IngredientData>> _ownedIngredientsAssetReferences;
        private List<AssetReferenceT<PotionData>> _ownedPotionsReferences;
        private List<AssetReferenceGameObject> _ownedTablesReferences;
        private List<AssetReferenceGameObject> _ownedEnvironmentReferences;
        
        private PlayerProgress _playerProgress;

        public void Initialize(PlayerProgress progress)
        {
            _playerProgress = progress;
            
            _ownedIngredientsAssetReferences = progress
                .IngredientsGuids
                .Select(guid => new AssetReferenceT<IngredientData>(guid))
                .ToList();

            _ownedPotionsReferences = progress.PlayerItems.PotionGuids
                .Select(guid => new AssetReferenceT<PotionData>(guid))
                .ToList();
            _ownedTablesReferences = progress.PlayerItems.TableGuids
                .Select(guid => new AssetReferenceGameObject(guid))
                .ToList();
            _ownedEnvironmentReferences = progress.PlayerItems.EnvironmentGuids
                .Select(guid => new AssetReferenceGameObject(guid))
                .ToList();
            
            ChosenPotionDataReference = _ownedPotionsReferences.Find(
                reference => reference.AssetGUID == progress.PotionDataGuid);
            ChosenAlchemyTablePrefabReference = _ownedTablesReferences.Find(
                reference => reference.AssetGUID == progress.AlchemyTablePrefabGuid);
            ChosenEnvironmentPrefabReference = _ownedEnvironmentReferences.Find(
                reference => reference.AssetGUID == progress.EnvironmentPrefabGuid);
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
            if (_playerProgress.IngredientsGuids.Contains(ingredientReference.AssetGUID))
            {
                return;
            }
            
            _playerProgress.IngredientsGuids.Add(ingredientReference.AssetGUID);
            _ownedIngredientsAssetReferences.Add(ingredientReference);
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

        public void AddNewPotion(AssetReferenceT<PotionData> potionDataReference)
        {
            if (IsPlayerOwnPotion(potionDataReference))
            {
                Debug.LogError($"Player already have such potion - {potionDataReference}");
                return;
            }
            
            _ownedPotionsReferences.Add(potionDataReference);
            _playerProgress.PlayerItems.PotionGuids.Add(potionDataReference.AssetGUID);
        }

        public void AddNewTable(AssetReferenceGameObject alchemyTableReference)
        {
            if (IsPlayerOwnTable(alchemyTableReference))
            {
                Debug.LogError($"Player already have such table - {alchemyTableReference}");
                return;
            }
            
            _ownedTablesReferences.Add(alchemyTableReference);
            _playerProgress.PlayerItems.TableGuids.Add(alchemyTableReference.AssetGUID);
        }

        public void AddNewEnvironment(AssetReferenceGameObject environmentReference)
        {
            if (IsPlayerOwnEnvironment(environmentReference))
            {
                Debug.LogError($"Player already have such environment - {environmentReference}");
                return;
            }
            
            _ownedEnvironmentReferences.Add(environmentReference);
            _playerProgress.PlayerItems.EnvironmentGuids.Add(environmentReference.AssetGUID);
        }

        public void SetChosenPotion(AssetReferenceT<PotionData> potionDataReference)
        {
            if (!IsPlayerOwnPotion(potionDataReference))
            {
                Debug.LogError($"Player does not own such potion - {potionDataReference}");
                return;
            }
            ChosenPotionDataReference = potionDataReference;
            _playerProgress.PotionDataGuid = potionDataReference.AssetGUID;
        }

        public void SetChosenTable(AssetReferenceGameObject alchemyTableReference)
        {
            if (!IsPlayerOwnTable(alchemyTableReference))
            {
                Debug.LogError($"Player does not own such table - {alchemyTableReference}");
                return;
            }
            ChosenAlchemyTablePrefabReference = alchemyTableReference;
            _playerProgress.AlchemyTablePrefabGuid = alchemyTableReference.AssetGUID;
        }

        public void SetChosenEnvironment(AssetReferenceGameObject environmentReference)
        {
            if (!IsPlayerOwnEnvironment(environmentReference))
            {
                Debug.LogError($"Player does not own such environment - {environmentReference}");
                return;
            }
            ChosenEnvironmentPrefabReference = environmentReference;
            _playerProgress.EnvironmentPrefabGuid = environmentReference.AssetGUID;

        }

        public bool IsPlayerOwnPotion(AssetReference potionDataReference) => 
            _playerProgress.PlayerItems.PotionGuids.Contains(potionDataReference.AssetGUID);

        public bool IsPlayerOwnTable(AssetReference alchemyTableReference) => 
            _playerProgress.PlayerItems.TableGuids.Contains(alchemyTableReference.AssetGUID);

        public bool IsPlayerOwnEnvironment(AssetReference environmentReference) => 
            _playerProgress.PlayerItems.EnvironmentGuids.Contains(environmentReference.AssetGUID);

        public bool IsCoinsEnoughFor(int itemPrice) => CoinsAmount >= itemPrice;

        public bool IsReputationEnoughFor(int itemPrice) => ReputationAmount >= itemPrice;
    }
}