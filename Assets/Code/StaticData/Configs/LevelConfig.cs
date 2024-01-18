using UnityEngine;

namespace Code.StaticData.Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "StaticData/LevelConfigs")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private Vector3 _tablePosition;
        [SerializeField] private Vector3 _environmentPosition;

        public string SceneName => _sceneName;
        public Vector3 TablePosition => _tablePosition;
        public Vector3 EnvironmentPosition => _environmentPosition;
    }
}