using Code.Logic.Orders;
using Code.StaticData;
using Code.StaticData.Orders;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.Services.Factories
{
    public interface IPotionOrderFactory
    {
        UniTask<PotionOrder> CreateOrderAsync(PotionOrderDifficulty orderDifficulty, PotionOrderType orderType);
    }
}