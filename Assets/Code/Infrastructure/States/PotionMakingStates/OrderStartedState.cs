using Code.Data;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.StaticData;
using Code.Logic.Orders;
using Code.Logic.PotionMaking;
using Code.StaticData;
using Code.UI.AwaitingOverlays;
using Code.UI.PotionMakingUI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.States.PotionMakingStates
{
    public class OrderStartedState : IPayloadState<PotionOrder>
    {
        private readonly SelectedPotionOrderHolder _selectedOrderHolder;
        private readonly IStaticDataService _staticDataService;
        private readonly IAlchemyTableFactory _tableFactory;
        private readonly IUIFactory _uiFactory;
        private readonly IAwaitingOverlay _awaitingOverlay;
        
        private PotionMakingPopup _potionMakingPopup;
        private AlchemyTable _alchemyTable;

        public OrderStartedState(IStaticDataService staticDataService, 
            IAlchemyTableFactory tableFactory,
            IUIFactory uiFactory,
            IAwaitingOverlay awaitingOverlay,
            SelectedPotionOrderHolder selectedOrderHolder)
        {
            _uiFactory = uiFactory;
            _tableFactory = tableFactory;
            _staticDataService = staticDataService;
            _awaitingOverlay = awaitingOverlay;
            _selectedOrderHolder = selectedOrderHolder;
        }
        public async UniTask Enter(PotionOrder payload)
        {
            _selectedOrderHolder.PutOrder(payload);
            
            LevelConfig levelConfig = _staticDataService.GetLevelConfigBySceneName(ResourcesPaths.PotionMakingSceneAddress);
            
            _alchemyTable = await _tableFactory.CreateTableAsync(levelConfig.TablePosition);
            _potionMakingPopup = await _uiFactory.CreatePotionMakingPopup(_alchemyTable);
           
            _awaitingOverlay.Hide().Forget();
        }

        public UniTask Exit()
        {
            Object.Destroy(_alchemyTable.gameObject);
            Object.Destroy(_potionMakingPopup.gameObject);
            return UniTask.CompletedTask;
        }
    }
}