using Code.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "WindowConfig", menuName = "StaticData/Windows/WindowConfig")]
    public class WindowConfig : ScriptableObject
    {
        [SerializeField] private WindowType _type;
        [SerializeField] private AssetReferenceGameObject _prefabReference;

        public WindowType Type => _type;
        public AssetReferenceGameObject PrefabReference => _prefabReference;
    }
}