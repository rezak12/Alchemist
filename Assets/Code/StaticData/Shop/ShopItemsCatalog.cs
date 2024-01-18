using System.Collections.Generic;
using UnityEngine;

namespace Code.StaticData.Shop
{
    [CreateAssetMenu(fileName = "ShopItemsCatalog", menuName = "StaticData/Shop/Catalog")]
    public class ShopItemsCatalog : ScriptableObject
    {
        [SerializeField] private List<PotionShopItem> _potionItems;
        [SerializeField] private List<TableShopItem> _tableItems;
        [SerializeField] private List<EnvironmentShopItem> _environmentItems;

        public IReadOnlyCollection<PotionShopItem> PotionItems => _potionItems;
        public IReadOnlyCollection<TableShopItem> TableItems => _tableItems;
        public IReadOnlyCollection<EnvironmentShopItem> EnvironmentItems => _environmentItems;
    }
}