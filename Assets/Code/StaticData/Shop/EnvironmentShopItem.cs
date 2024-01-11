using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.StaticData.Shop
{
    [CreateAssetMenu(fileName = "EnvironmentShopItem", menuName = "StaticData/Shop/Environment")]
    public class EnvironmentShopItem : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int RequiredCoins { get; private set; }
        [field: SerializeField] public int RequiredReputation { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject Environment { get; private set; }
    }
}