using System.Reflection;
using Code.Logic.PotionMaking;
using Code.StaticData;
using Code.StaticData.Configs;
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
        private const string EnvironmentPositionFieldName = "_environmentPosition";

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
            SetSceneName(config);
            SetAlchemyTablePoint(config);
            SetEnvironmentPoint(config);
        }

        private void SetSceneName(LevelConfig config)
        {
            string activeSceneName = SceneManager.GetActiveScene().name;
            
            FieldInfo sceneNameField = typeof(LevelConfig).GetField(
                SceneNameFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (sceneNameField == null)
            {
                Debug.LogError("Scene name field name has been changed science last amend to editor tool");
                return;
            }
            
            sceneNameField.SetValue(config, activeSceneName);
        }
        
        private void SetAlchemyTablePoint(LevelConfig config)
        {
            var tableSpawnPoint = FindObjectOfType<AlchemyTableSpawnPoint>();
            if (tableSpawnPoint == null)
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
            
            tablePositionField.SetValue(config, tableSpawnPoint.transform.position);
        }
        
        private void SetEnvironmentPoint(LevelConfig config)
        {
            var environmentSpawnPoint = FindObjectOfType<EnvironmentSpawnPoint>();
            if (environmentSpawnPoint == null)
            {
                Debug.LogError("There are not EnvironmentSpawnPoint in scene");
                return;
            }

            FieldInfo environmentPositionField = typeof(LevelConfig).GetField(
                EnvironmentPositionFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (environmentPositionField == null)
            {
                Debug.LogError("Environment position field name has been changed science last amend to editor tool");
                return;
            }
            
            environmentPositionField.SetValue(config, environmentSpawnPoint.transform.position);
        }
    }
}