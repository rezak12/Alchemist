using Code.Logic.Orders;
using Code.StaticData;

namespace Code.Infrastructure.Services.Factories
{
    public interface IPotionOrderFactory
    {
        PotionOrder CreateOrder(PotionOrderDifficulty orderDifficulty, PotionOrderType orderType);
    }
}