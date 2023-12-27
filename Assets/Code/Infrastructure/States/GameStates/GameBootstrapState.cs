using Code.Data;
using Code.Infrastructure.Services.Ambient;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.Settings;
using Code.Infrastructure.Services.SFX;
using Code.Infrastructure.Services.StaticData;
using Code.UI.AwaitingOverlays;
using Cysharp.Threading.Tasks;
using UnityEngine.Audio;

namespace Code.Infrastructure.States.GameStates
{
    public class GameBootstrapState : IState
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly AwaitingOverlayProxy _awaitingOverlay;
        private readonly GameStateMachine _stateMachine;
        private readonly ISFXProvider _sfxProvider;
        private readonly IAmbientPlayer _ambientPlayer;
        private readonly ISettingsService _settingsService;

        public GameBootstrapState(
            IAssetProvider assetProvider, 
            IStaticDataService staticDataService,
            AwaitingOverlayProxy awaitingOverlay,
            GameStateMachine stateMachine, 
            ISFXProvider sfxProvider, 
            IAmbientPlayer ambientPlayer, 
            ISettingsService settingsService)
        {
            _awaitingOverlay = awaitingOverlay;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _stateMachine = stateMachine;
            _sfxProvider = sfxProvider;
            _ambientPlayer = ambientPlayer;
            _settingsService = settingsService;
        }

        public async UniTask Enter()
        {
            await _assetProvider.InitializeAsync();
            await _awaitingOverlay.InitializeAsync();
            await _staticDataService.InitializeAsync();
            await _sfxProvider.InitializeAsync();
            await _ambientPlayer.InitializeAsync();
            await _settingsService.InitializeAsync(await _assetProvider.LoadAsync<AudioMixer>(
                ResourcesAddresses.AudioMixerAddress));

            _ambientPlayer.StartPlayingLoop();
            await _awaitingOverlay.Show("Loading progress");
            await _stateMachine.Enter<LoadProgressState>();
        }

        public UniTask Exit() => UniTask.CompletedTask;
    }
}