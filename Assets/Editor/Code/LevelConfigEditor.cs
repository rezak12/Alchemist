using System.Reflection;
using Code.Logic.PotionMaking;
using Code.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor.Code
{
    [CustomEditor(typeof(LevelConfig))]
    public class LevelConfigEditor : UnityEditor.Editor
    {
        private const string SceneNameFieldName = "_sceneName";
        private const string AlchemyTablePositionFieldName = "_tablePosition";

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            LevelConfig config = (LevelConfig)target;

            if (GUILayout.Button("Update Scene Info"))
            {
                UpdateConfigInfo(config);
                EditorUtility.SetDirty(config);
            }
        }

        private void UpdateConfigInfo(LevelConfig config)
        {
            var activeSceneName = SceneManager.GetActiveScene().name;
            
            FieldInfo sceneNameField = typeof(LevelConfig).GetField(
                SceneNameFieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (sceneNameField == null)
            {
                Debug.LogError("Scene name field name has been changed science last amend to editor tool");
                return;
            }
            sceneNameField.SetValue(config, activeSceneName);
            

            var spawnPoint = FindObjectOfType<AlchemyTableSpawnPoint>();
            if (spawnPoint == null)
            {
                Debug.LogError("There are not AlchemyTableSpawnPoint in scene");
                return;
            }
            FieldInfo tablePositionField = typeof(LevelConfig).GetField(
                AlchemyTablePositionFieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (tablePositionField == null)
            {
                Debug.LogError("Alchemy table position field name has been changed science last amend to editor tool");
                return;
            }
            tablePositionField.SetValue(config, spawnPoint.transform.position);
        }
    }
}