using System.IO;
using Code.Data;
using Code.Infrastructure.Services.ProgressServices;
using UnityEngine;

namespace Code.Infrastructure.Services.SaveLoadService
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly string _saveFilePath = $"{Application.persistentDataPath}/{Constants.SaveFileName}";

        public void SaveProgress(PlayerProgress progress)
        {
            var json = progress.ToJson();
            File.WriteAllText(_saveFilePath, json);
        }
        
        public PlayerProgress LoadProgress()
        {
            if (!File.Exists(_saveFilePath)) return null;
            return File.ReadAllText(_saveFilePath).FromJson<PlayerProgress>();
        }
    }
}