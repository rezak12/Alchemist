using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.Settings;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.Services.SaveLoadService
{
    public interface ISaveLoadService
    {
        UniTask SaveProgress(PlayerProgress progress);
        UniTask SaveSettings(GameSettings settings);
        UniTask<PlayerProgress> LoadProgress();
        UniTask<GameSettings> LoadSettings();
    }
}