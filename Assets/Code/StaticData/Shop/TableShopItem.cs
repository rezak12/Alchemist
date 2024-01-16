using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.StaticData.Shop
{
    [CreateAssetMenu(fileName = "TableShopItem", menuName = "StaticData/Shop/Table")]
    public class TableShopItem : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int RequiredCoins { get; private set; }
        [field: SerializeField] public int RequiredReputation { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject TableReference { get; private set; }
    }
}