using Code.Attributes;
using UnityEngine;

namespace Code.StaticData.Configs
{
    [CreateAssetMenu(fileName = "VersionInfo", menuName = "StaticData/VersionInfo")]
    public class VersionInfo : ScriptableObject
    {
        [SerializeField, ReadOnly] private string _version;
        public string Version => _version;
        [field: SerializeField] public string Label { get; private set; }
        [field: SerializeField] public string AdditionInformation { get; private set; }
    }
}