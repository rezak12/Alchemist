using System.Collections.Generic;
using UnityEngine;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "WindowsCatalog", menuName = "StaticData/Windows/WindowsCatalog", order = 0)]
    public class WindowsCatalog : ScriptableObject
    {
        [SerializeField] private WindowConfig[] _configs;
        public IEnumerable<WindowConfig> Configs => _configs;
    }
}