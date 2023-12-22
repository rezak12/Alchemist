using System.IO;
using Code.Data;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.Settings;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.SaveLoadService
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly string _progressFilePath = $"{Application.persistentDataPath}/{Constants.ProgressSaveFileName}";
        private readonly string _settingsFilePath = $"{Application.persistentDataPath}/{Constants.SettingsSaveFileName}";

        public UniTask SaveProgress(PlayerProgress progress) => SaveAsJson(_progressFilePath, progress);

        public UniTask SaveSettings(GameSettings settings) => SaveAsJson(_settingsFilePath, settings);

        public UniTask<PlayerProgress> LoadProgress() => ReadAsJson<PlayerProgress>(_progressFilePath);

        public UniTask<GameSettings> LoadSettings() => ReadAsJson<GameSettings>(_settingsFilePath);

        private async UniTask SaveAsJson(string path, object objectToSave) => 
            await File.WriteAllTextAsync(path, objectToSave.ToJson()).AsUniTask();

        private async UniTask<TSave> ReadAsJson<TSave>(string path) where TSave : class
        {
            if (!File.Exists(path)) return null;
            string json = await File.ReadAllTextAsync(path).AsUniTask();
            return json.FromJson<TSave>();
        }
    }
}