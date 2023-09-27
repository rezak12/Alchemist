using System;
using Code.Infrastructure.Services.CoroutineRunner;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.StaticData;
using Code.StaticData;
using Cysharp.Threading.Tasks;

namespace Code.Logic.Orders
{
    public class PotionOrdersHandler
    {
        public PotionOrder CurrentOrder { get; private set; }
        public event Action NewOrderHandled;
        
        private readonly IPotionOrderFactory _potionOrderFactory;
        private readonly IStaticDataService _staticDataService;
        
        public PotionOrdersHandler(
            IPotionOrderFactory potionOrderFactory, 
            IStaticDataService staticDataService, 
            ICoroutineRunner coroutineRunner)
        {
            _potionOrderFactory = potionOrderFactory;
            _staticDataService = staticDataService;
        }

        public async UniTaskVoid HandleNewOrder()
        {
            PotionOrderDifficulty difficulty = _staticDataService.GetRandomPotionOrderDifficulty();
            PotionOrderType type = _staticDataService.GetRandomPotionOrderType();

            CurrentOrder = await _potionOrderFactory.CreateOrderAsync(difficulty, type);
            NewOrderHandled?.Invoke();
        }
    }
}