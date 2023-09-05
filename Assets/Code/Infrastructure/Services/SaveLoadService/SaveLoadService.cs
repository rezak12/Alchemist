using Code.Data;
using Code.Infrastructure.Services.ProgressServices;
using UnityEngine;

namespace Code.Infrastructure.Services.SaveLoadService
{
    public class SaveLoadService : ISaveLoadService
    {
        public void SaveProgress(PlayerProgress progress)
        {
            PlayerPrefs.SetString("Progress", progress.ToJson());
        }
        
        public PlayerProgress LoadProgress()
        {
            return PlayerPrefs.GetString("Progress")?.FromJson<PlayerProgress>();
        }
    }
}