using System;
using System.Collections.Generic;
using Code.Logic.Potions;
using Code.StaticData;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.ProgressServices
{
    public interface IPersistentProgressService
    {
        int CoinsAmount { get; }
        int ReputationAmount { get; }
        HashSet<AssetReferenceT<IngredientData>> PlayerIngredientsAssetReferences { get; }
        AssetReferenceT<Potion> CurrentPlayerPotionPrefabReference { get; }

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