using Code.Data;
using Code.Infrastructure.Services.ProgressServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Infrastructure.Services.SaveLoadService
{
    public class SaveLoadService : ISaveLoadService
    {
        public void SaveProgress(PlayerProgress progress)
        {
            PlayerPrefs.SetString("Progress", progress.ToJson());
        }
        
        [CanBeNull]
        public PlayerProgress LoadProgress()
        {
            return PlayerPrefs.GetString("Progress")?.FromJson<PlayerProgress>();
        }
    }
}