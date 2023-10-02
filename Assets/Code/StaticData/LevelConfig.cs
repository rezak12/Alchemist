using UnityEngine;
using UnityEngine.Serialization;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "StaticData/LevelConfigs")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private Vector3 _tablePosition;

        public string SceneName => _sceneName;
        public Vector3 TablePosition => _tablePosition;
    }
}