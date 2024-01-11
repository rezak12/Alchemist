using Code.StaticData.Potions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.StaticData.Shop
{
    [CreateAssetMenu(fileName = "PotionShopItem", menuName = "StaticData/Shop/Potion")]
    public class PotionShopItem : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int RequiredCoins { get; private set; }
        [field: SerializeField] public int RequiredReputation { get; private set; }
        [field: SerializeField] public AssetReferenceT<PotionData> Potion { get; private set; }
        
    }
}