using Code.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "WindowConfig", menuName = "StaticData/Windows/WindowConfig")]
    public class PopupConfig : ScriptableObject
    {
        [SerializeField] private PopupType _type;
        [SerializeField] private AssetReferenceGameObject _prefabReference;

        public PopupType Type => _type;
        public AssetReferenceGameObject PrefabReference => _prefabReference;
    }
}