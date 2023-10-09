using Code.Data;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.SaveLoadService;
using Code.StaticData;
using Code.UI.AwaitingOverlays;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States.GameStates
{
    public class LoadProgressState : IState
    {
        private readonly IAssetProvider _assetProvider;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPersistentProgressService _progressService;
        private readonly IAwaitingOverlay _awaitingOverlay;

        private readonly GameStateMachine _stateMachine;

        public LoadProgressState(
            IAssetProvider assetProvider, 
            ISaveLoadService saveLoadService, 
            IPersistentProgressService progressService,
            IAwaitingOverlay awaitingOverlay,
            GameStateMachine stateMachine)
        {
            _assetProvider = assetProvider;
            _saveLoadService = saveLoadService;
            _progressService = progressService;
            _awaitingOverlay = awaitingOverlay;
            _stateMachine = stateMachine;
        }

        public async UniTask Enter()
        {
            _awaitingOverlay.Show("Loading...");
            _progressService.Initialize(await LoadOrCreateNewSave());
            await _stateMachine.Enter<MainMenuState>();
        }

        public UniTask Exit()
        {
            return default;
        }

        private async UniTask<PlayerProgress> LoadOrCreateNewSave()
        {
            PlayerProgress progress = _saveLoadService.LoadProgress();
            if (progress != null) return progress;
            
            var newSaveData = await _assetProvider.LoadAsync<FirstSaveData>(ResourcesPaths.NewSaveDataAddress);
            progress = new PlayerProgress(
                newSaveData.CoinsAmount,
                newSaveData.ReputationAmount,
                newSaveData.IngredientsGUIDs,
                newSaveData.PotionPrefabGUID,
                newSaveData.AlchemyTablePrefabGUID);

            return progress;
        }
    }
}