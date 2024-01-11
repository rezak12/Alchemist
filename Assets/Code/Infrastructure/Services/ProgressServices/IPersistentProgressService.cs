using System;
using System.Collections.Generic;
using Code.StaticData;
using Code.StaticData.Ingredients;
using Code.StaticData.Potions;
using Code.StaticData.Shop;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.ProgressServices
{
    public interface IPersistentProgressService
    {
        int CoinsAmount { get; }
        int ReputationAmount { get; }
        IEnumerable<AssetReferenceT<IngredientData>> OwnedIngredientsAssetReferences { get; }
        AssetReferenceT<PotionData> ChosenPotionDataReference { get; }
        AssetReferenceGameObject ChosenAlchemyTablePrefabReference { get; }
        AssetReferenceGameObject ChosenEnvironmentPrefabReference { get; }
        
        IReadOnlyCollection<AssetReferenceT<PotionData>> OwnedPotionsReferences { get; }
        IReadOnlyCollection<AssetReferenceGameObject> OwnedTablesReferences { get; }
        IReadOnlyCollection<AssetReferenceGameObject> OwnedEnvironmentsReferences { get; }

        event Action CoinsAmountChanged;
        event Action ReputationAmountChanged;
        
        void Initialize(PlayerProgress progress);
        PlayerProgress GetProgress();
        
        void AddCoins(int amount);
        void AddReputation(int amount);
        void AddNewIngredient(AssetReferenceT<IngredientData> ingredientReference);

        void RemoveCoins(int amount);
        void RemoveReputation(int amount);

        void AddNewPotion(AssetReferenceT<PotionData> potionDataReference);
        void AddNewTable(AssetReferenceGameObject alchemyTableReference);
        void AddNewEnvironment(AssetReferenceGameObject environmentReference);
        
        bool IsCoinsEnoughFor(int itemPrice);
        bool IsReputationEnoughFor(int itemPrice);
    }
}