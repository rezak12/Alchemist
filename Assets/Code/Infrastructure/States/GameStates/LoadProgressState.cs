using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.SaveLoadService;
using Code.StaticData;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States.GameStates
{
    public class LoadProgressState : IState
    {
        private IAssetProvider _assetProvider;
        private const string NewSaveDataAddress = "NewSaveData";
        
        private ISaveLoadService _saveLoadService;
        private IPersistentProgressService _progressService;

        public async UniTask Enter()
        {
            _progressService.Initialize(await LoadOrCreateNewSave());
        }

        public UniTask Exit()
        {
            return default;
        }

        private async UniTask<PlayerProgress> LoadOrCreateNewSave()
        {
            PlayerProgress progress = _saveLoadService.LoadProgress();
            if (progress != null) return progress;
            
            var newSaveData = await _assetProvider.LoadAsync<FirstSaveData>(NewSaveDataAddress);
            progress = new PlayerProgress(
                newSaveData.CoinsAmount,
                newSaveData.ReputationAmount,
                newSaveData.IngredientsGUIDs,
                newSaveData.PotionPrefabGUID);

            return progress;
        }
    }
}