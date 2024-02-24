using Code.Data;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.Settings;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.SaveLoadService
{
    public class PrefsSaveLoadService : ISaveLoadService
    {
        public UniTask SaveProgress(PlayerProgress progress)
        {
            SaveAsJson(Constants.ProgressSaveFileName, progress);
            return UniTask.CompletedTask;
        }

        public UniTask SaveSettings(GameSettings settings)
        {
            SaveAsJson(Constants.SettingsSaveFileName, settings);
            return UniTask.CompletedTask;
        }

        public UniTask<PlayerProgress> LoadProgress()
        {
            var settings = ReadAsJson<PlayerProgress>(Constants.ProgressSaveFileName);
            return UniTask.FromResult(settings);
        }

        public UniTask<GameSettings> LoadSettings()
        {
            var settings = ReadAsJson<GameSettings>(Constants.SettingsSaveFileName);
            return UniTask.FromResult(settings);
        }

        private void SaveAsJson(string path, object objectToSave) => 
            PlayerPrefs.SetString(path, objectToSave.ToJson());

        private TSave ReadAsJson<TSave>(string path) where TSave : class => 
            PlayerPrefs.GetString(path)?.FromJson<TSave>();
    }
}