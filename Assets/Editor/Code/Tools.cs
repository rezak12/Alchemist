using Code.Data;
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

        [MenuItem("Tools/DeleteSaveFile")]
        public static void DeleteSaveFile()
        {
            var path = $"{Application.persistentDataPath}/{Constants.SaveFileName}";
            if(!File.Exists(path)) return;
            File.Delete(path);
        }
    }
}