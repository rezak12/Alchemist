using Code.Logic.Orders;
using Code.StaticData;

namespace Code.Infrastructures.Services.Factories
{
    public interface IPotionOrderFactory
    {
        PotionOrder CreateOrder(PotionOrderDifficulty orderDifficulty, PotionOrderType orderType);
    }
}