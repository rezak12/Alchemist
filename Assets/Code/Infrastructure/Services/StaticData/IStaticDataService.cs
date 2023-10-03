using Code.StaticData;
using Code.UI;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        UniTask InitializeAsync();
        public PopupConfig GetPopupByType(PopupType type);
        public PotionOrderType GetRandomPotionOrderType();
        public PotionOrderDifficulty GetRandomPotionOrderDifficulty();
        LevelConfig GetLevelConfigBySceneName(string sceneName);
    }
}