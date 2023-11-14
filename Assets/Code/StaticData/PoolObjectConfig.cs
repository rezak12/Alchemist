using Code.Infrastructure.Services.Pool;
using Code.Infrastructure.Services.VFX;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "NewPoolObjectData", menuName = "StaticData/PoolObjectData")]
    public class PoolObjectConfig : ScriptableObject
    {
        [SerializeField] private PoolObjectType _type;
        [SerializeField] private int _startCapacity;
        [SerializeField] private AssetReferenceGameObject _assetReference;

        public PoolObjectType Type => _type;
        public int StartCapacity => _startCapacity;
        public AssetReferenceGameObject AssetReference => _assetReference;
    }
}