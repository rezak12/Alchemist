using Code.Infrastructure.Services.ProgressServices;
using JetBrains.Annotations;

namespace Code.Infrastructure.Services.SaveLoadService
{
    public interface ISaveLoadService
    {
        void SaveProgress(PlayerProgress progress);
        [CanBeNull] PlayerProgress LoadProgress();
    }
}