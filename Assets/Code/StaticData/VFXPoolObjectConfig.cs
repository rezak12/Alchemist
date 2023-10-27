using Code.Infrastructure.Services.FX;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "NewVFXPoolObjectData", menuName = "StaticData/VFXPoolObjectData")]
    public class VFXPoolObjectConfig : ScriptableObject
    {
        [SerializeField] private VFXType _type;
        [SerializeField] private int _startCapacity;
        [SerializeField] private AssetReferenceGameObject _assetReference;

        public VFXType Type => _type;
        public int StartCapacity => _startCapacity;
        public AssetReferenceGameObject AssetReference => _assetReference;
    }
}