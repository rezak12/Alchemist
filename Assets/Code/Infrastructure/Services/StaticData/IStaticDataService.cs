using Code.StaticData;
using Code.UI;

namespace Code.Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        public WindowConfig GetWindowByType(WindowType type);
        public PotionOrderType GetRandomPotionOrderType();
        public PotionOrderDifficulty GetRandomPotionOrderDifficulty();
    }
}