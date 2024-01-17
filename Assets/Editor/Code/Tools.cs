using Code.Data;
using Code.Infrastructure.Services.ProgressServices;
using UnityEditor;
using UnityEngine;
using File = System.IO.File;

namespace Editor.Code
{
    public class Tools
    {
        [MenuItem("Tools/Clear Prefs")]
        public static void ClearPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        [MenuItem("Tools/Delete Save File")]
        public static void DeleteSaveFile()
        {
            var path = $"{Application.persistentDataPath}/{Constants.ProgressSaveFileName}";
            if(!File.Exists(path)) return;
            File.Delete(path);
        }

        [MenuItem("Tools/Give coins and reputation")]
        public static void GiveCoinsAndReputation()
        {
            var path = $"{Application.persistentDataPath}/{Constants.ProgressSaveFileName}";
            if (!File.Exists(path))
            {
                Debug.Log("There is not player progress saved. Can not give coins and reputation");
                return;
            }

            var playerProgress = File.ReadAllText(path).FromJson<PlayerProgress>();
            playerProgress.CoinsAmount = 999;
            playerProgress.ReputationAmount = 999;
            File.WriteAllText(path, playerProgress.ToJson());
        }
    }
}