using Code.Logic.Orders;
using Code.StaticData;

namespace Code.Infrastructures.Services
{
    public interface IPotionOrderFactory
    {
        PotionOrder CreateOrder(PotionOrderDifficulty orderDifficulty, PotionOrderType orderType);
    }
}