using System;
using System.Collections.Generic;
using Code.StaticData;
using Code.StaticData.Ingredients;
using Code.StaticData.Potions;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.ProgressServices
{
    public interface IPersistentProgressService
    {
        int CoinsAmount { get; }
        int ReputationAmount { get; }
        List<AssetReferenceT<IngredientData>> PlayerIngredientsAssetReferences { get; }
        AssetReferenceT<PotionData> ChosenPotionDataReference { get; }
        AssetReferenceGameObject ChosenAlchemyTablePrefabReference { get; }
        AssetReferenceGameObject ChosenEnvironmentPrefabReference { get; }

        event Action CoinsAmountChanged;
        event Action ReputationAmountChanged;
        
        void Initialize(PlayerProgress progress);
        PlayerProgress GetProgress();
        
        void AddCoins(int amount);
        void AddReputation(int amount);
        void AddNewIngredient(AssetReferenceT<IngredientData> ingredient);

        void RemoveCoins(int amount);
        void RemoveReputation(int amount);
        
        bool IsCoinsEnoughFor(int itemPrice);
        bool IsReputationEnoughFor(int itemPrice);
    }
}