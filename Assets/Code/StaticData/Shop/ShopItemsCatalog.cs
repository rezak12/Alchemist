using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.StaticData.Shop
{
    [CreateAssetMenu(fileName = "ShopItemsCatalog", menuName = "StaticData/Shop/Catalog")]
    public class ShopItemsCatalog : ScriptableObject
    {
        [SerializeField] private List<AssetReferenceT<PotionShopItem>> _potionItems;
        [SerializeField] private List<AssetReferenceT<TableShopItem>> _tableItems;
        [SerializeField] private List<AssetReferenceT<EnvironmentShopItem>> _environmentItems;

        public IReadOnlyCollection<AssetReferenceT<PotionShopItem>> PotionItems => _potionItems;
        public IReadOnlyCollection<AssetReferenceT<TableShopItem>> TableItems => _tableItems;
        public IReadOnlyCollection<AssetReferenceT<EnvironmentShopItem>> EnvironmentItems => _environmentItems;
    }
}