using System;
using System.Collections;
using Code.Infrastructure.Services.CoroutineRunner;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.StaticData;
using Code.StaticData;

namespace Code.Logic.Orders
{
    public class PotionOrdersHandler
    {
        public PotionOrder CurrentOrder { get; private set; }
        public event Action NewOrderHandled;
        
        private readonly IPotionOrderFactory _potionOrderFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly ICoroutineRunner _coroutineRunner;


        public PotionOrdersHandler(
            IPotionOrderFactory potionOrderFactory, 
            IStaticDataService staticDataService, 
            ICoroutineRunner coroutineRunner)
        {
            _potionOrderFactory = potionOrderFactory;
            _staticDataService = staticDataService;
            _coroutineRunner = coroutineRunner;
        }

        public void HandleNewOrder()
        {
            _coroutineRunner.StartCoroutine(HandleNewOrderCoroutine());
        }

        private IEnumerator HandleNewOrderCoroutine()
        {
            PotionOrderDifficulty difficulty = _staticDataService.GetRandomPotionOrderDifficulty();
            PotionOrderType type = _staticDataService.GetRandomPotionOrderType();

            var task = _potionOrderFactory.CreateOrderAsync(difficulty, type);
            yield return task;

            CurrentOrder = task.Result;
            NewOrderHandled?.Invoke();
        }
    }
}