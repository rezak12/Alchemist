using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.SceneLoader;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States.GameStates
{
    public class MainMenuState : IState
    {
        private const string MainMenuSceneAddress = "MainMenu";
        
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private readonly GameStateMachine _stateMachine;

        public MainMenuState(ISceneLoader sceneLoader, IUIFactory uiFactory, GameStateMachine stateMachine)
        {
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _stateMachine = stateMachine;
        }

        public async UniTask Enter()
        {
            await _sceneLoader.LoadAsync(MainMenuSceneAddress);
            await _uiFactory.CreateStorePopupAsync();
        }

        public UniTask Exit()
        {
            return default;
        }

        
    }
}