using Code.Infrastructure.Services.ProgressServices;

namespace Code.Infrastructure.Services.SaveLoadService
{
    public interface ISaveLoadService
    {
        void SaveProgress(PlayerProgress progress);
        PlayerProgress LoadProgress();
    }
}