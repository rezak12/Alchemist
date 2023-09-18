using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "PotionOrderDifficultyData", menuName = "StaticData/PotionOrders/OrderType")]
    public class PotionOrderType : ScriptableObject
    {
        [SerializeField] private string _name;
        
        [SerializeField, FormerlySerializedAs("_requirementPotionCharacteristics")]
        private List<AssetReferenceT<PotionCharacteristic>> _possibleRequirementPotionCharacteristics;
        
        [SerializeField] private List<AssetReferenceT<IngredientData>> _possibleRewardIngredients;

        public string Name => _name;
        public List<AssetReferenceT<PotionCharacteristic>> PossibleRequirementPotionCharacteristicsReferences => 
            _possibleRequirementPotionCharacteristics;
        public List<AssetReferenceT<IngredientData>> PossibleRewardIngredientsReferences => _possibleRewardIngredients;
    }
}