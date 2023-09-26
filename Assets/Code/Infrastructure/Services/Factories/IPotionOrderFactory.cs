using Code.Logic.Orders;
using Code.StaticData;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.Services.Factories
{
    public interface IPotionOrderFactory
    {
        UniTask<PotionOrder> CreateOrderAsync(PotionOrderDifficulty orderDifficulty, PotionOrderType orderType);
    }
}