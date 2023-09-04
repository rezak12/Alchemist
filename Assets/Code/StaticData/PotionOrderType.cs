using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "PotionOrderDifficultyData", menuName = "StaticData/PotionOrders/OrderType")]
    public class PotionOrderType : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private List<AssetReferenceT<PotionCharacteristic>> _requirementPotionCharacteristics;
        [SerializeField] private List<AssetReferenceT<IngredientData>> _possibleRewardIngredients;

        public string Name => _name;
        public List<AssetReferenceT<PotionCharacteristic>> RequirementPotionCharacteristicsReferences => 
            _requirementPotionCharacteristics;
        public List<AssetReferenceT<IngredientData>> PossibleRewardIngredientsReferences => _possibleRewardIngredients;
    }
}