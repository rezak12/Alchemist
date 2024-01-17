using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.StaticData.Configs
{
    [CreateAssetMenu(fileName = "AmbientReferencesCatalog", menuName = "StaticData/AmbientReferences", order = 0)]
    public class AmbientReferencesCatalog : ScriptableObject
    {
        [field: SerializeField] public List<AssetReferenceT<AudioClip>> AmbientReferences { get; private set; }
    }
}