using Code.StaticData;
using Code.UI;

namespace Code.Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        public PopupConfig GetPopupByType(PopupType type);
        public PotionOrderType GetRandomPotionOrderType();
        public PotionOrderDifficulty GetRandomPotionOrderDifficulty();
    }
}