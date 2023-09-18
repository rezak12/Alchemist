using UnityEngine;
using UnityEngine.Serialization;

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
        [FormerlySerializedAs("_minRequirementCharacteristicPoints")]
        [SerializeField] private int _minRequirementCharacteristicPointsAmount;
        
        [Tooltip("The maximum possible amount of requirement characteristic points for each characteristic")]
        [FormerlySerializedAs("_maxRequirementCharacteristicPoints")]
        [SerializeField] private int _maxRequirementCharacteristicPointsAmount;
        
        [Space, Header("Order Reward")]
        [SerializeField] private int _minCoinsAmountReward;
        [SerializeField] private int _maxCoinsAmountReward;
        [SerializeField] private int _minReputationAmountReward;
        [SerializeField] private int _maxReputationAmountReward;
        [SerializeField, Range(0, 100)] private int _ingredientAsRewardChance;

        [Space, Header("Order Punishment")] 
        [SerializeField] private int _minReputationAmountPunishment;
        [SerializeField] private int _maxReputationAmountPunishment;

        public string Name => _name;
        
        public int RequirementCharacteristicsAmount => _requirementCharacteristicsAmount;
        public int MinRequirementCharacteristicPointsAmount => _minRequirementCharacteristicPointsAmount;
        public int MaxRequirementCharacteristicPointsAmount => _maxRequirementCharacteristicPointsAmount;
        
        public int MinCoinsAmountReward => _minCoinsAmountReward;
        public int MaxCoinsAmountReward => _maxCoinsAmountReward;
        public int MinReputationAmountReward => _minReputationAmountReward;
        public int MaxReputationAmountReward => _maxReputationAmountReward;
        public int IngredientAsRewardChance => _ingredientAsRewardChance;

        public int MinReputationAmountPunishment => _minReputationAmountPunishment;
        public int MaxReputationAmountPunishment => _maxReputationAmountPunishment;
    }
}