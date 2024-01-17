using Code.Logic.Potions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.StaticData.Potions
{
    [CreateAssetMenu(fileName = "NewPotionData", menuName = "StaticData/PotionData")]
    public class PotionData : ScriptableObject
    {
        [SerializeField] private Potion _prefab;
        [SerializeField] private AssetReferenceT<AudioClip> _sfx;

        public Potion Prefab => _prefab;
        public AssetReferenceT<AudioClip> SFX => _sfx;
    }
}