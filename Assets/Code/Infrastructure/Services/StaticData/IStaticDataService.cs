using Code.Infrastructure.Services.Pool;
using Code.StaticData;
using Code.StaticData.Configs;
using Code.StaticData.Orders;
using Code.StaticData.Shop;
using Code.UI;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        UniTask InitializeAsync();
        PopupConfig GetPopupByType(PopupType type);
        PotionOrderType GetRandomPotionOrderType();
        PotionOrderDifficulty GetRandomPotionOrderDifficulty();
        LevelConfig GetLevelConfigBySceneName(string sceneName);
        PoolObjectConfig GetPoolConfigByType(PoolObjectType type);
        AmbientReferencesCatalog GetAmbientReferencesCatalog();
        VersionInfo GetVersionInfo();
        ShopItemsCatalog GetShopItemsCatalog();
    }
}