﻿using UnityEngine;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "PotionOrderDifficultyData", menuName = "StaticData/PotionOrders/OrderDifficulty")]
    public class PotionOrderDifficulty : ScriptableObject
    {
        [SerializeField] private string _name;
        
        [Space, Header("Order Requirements")]
        [Tooltip("Total sum of all requirement characteristics for potion")]
        [SerializeField] private int _requirementCharacteristicsAmount;
        [Tooltip("The minimum possible amount of requirement characteristic points for each characteristic")]
        [SerializeField] private int _minRequirementCharacteristicPoints;
        [Tooltip("The maximum possible amount of requirement characteristic points for each characteristic")]
        [SerializeField] private int _maxRequirementCharacteristicPoints;
        
        [Space, Header("Order Reward")]
        [SerializeField] private int _minCoinsAmountReward;
        [SerializeField] private int _maxCoinsAmountReward;
        [SerializeField] private int _minReputationAmountReward;
        [SerializeField] private int _maxReputationAmountReward;
        [SerializeField, Range(0, 100)] private int _ingredientAsRewardChance;

        public string Name => _name;
        
        public int RequirementCharacteristicsAmount => _requirementCharacteristicsAmount;
        public int MinRequirementCharacteristicPoints => _minRequirementCharacteristicPoints;
        public int MaxRequirementCharacteristicPoints => _maxRequirementCharacteristicPoints;
        
        public int MinCoinsAmountReward => _minCoinsAmountReward;
        public int MaxCoinsAmountReward => _maxCoinsAmountReward;
        public int MinReputationAmountReward => _minReputationAmountReward;
        public int MaxReputationAmountReward => _maxReputationAmountReward;
        public int IngredientAsRewardChance => _ingredientAsRewardChance;
    }
}