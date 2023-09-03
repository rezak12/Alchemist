using System;
using UnityEngine;

namespace Code.Infrastructure.Services.ProgressServices
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public int CoinsAmount => _playerProgress.CoinsAmount;
        public int ReputationAmount => _playerProgress.ReputationAmount;

        public event Action CoinsAmountChanged;
        public event Action ReputationAmountChanged;
        
        private readonly PlayerProgress _playerProgress;

        public PersistentProgressService(PlayerProgress playerProgress)
        {
            _playerProgress = playerProgress;
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

        public bool IsCoinsEnoughFor(int itemPrice)
        {
            return CoinsAmount >= itemPrice;
        }
        
        public bool IsReputationEnoughFor(int itemPrice)
        {
            return CoinsAmount >= itemPrice;
        }
    }

    public class PlayerProgress
    {
        public int CoinsAmount;
        public int ReputationAmount;

        public PlayerProgress(int coinsAmount, int reputationAmount)
        {
            CoinsAmount = coinsAmount;
            ReputationAmount = reputationAmount;
        }
    }
}