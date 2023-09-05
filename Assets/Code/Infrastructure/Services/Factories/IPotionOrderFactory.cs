using System.Threading.Tasks;
using Code.Logic.Orders;
using Code.StaticData;

namespace Code.Infrastructure.Services.Factories
{
    public interface IPotionOrderFactory
    {
        Task<PotionOrder> CreateOrderAsync(PotionOrderDifficulty orderDifficulty, PotionOrderType orderType);
    }
}