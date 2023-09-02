using System.Collections.Generic;
using UnityEngine;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "PotionOrderDifficultyData", menuName = "StaticData/PotionOrders/OrderType")]
    public class PotionOrderType : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private List<PotionCharacteristic> _requirementPotionCharacteristics;
        [SerializeField] private List<IngredientData> _possibleRewardIngredients;

        public string Name => _name;
        public List<PotionCharacteristic> RequirementPotionCharacteristics => _requirementPotionCharacteristics;
        public List<IngredientData> PossibleRewardIngredients => _possibleRewardIngredients;
    }
}