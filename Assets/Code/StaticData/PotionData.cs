using Code.Logic.Potions;
using UnityEngine;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "NewPotionData", menuName = "StaticData/PotionData")]
    public class PotionData : ScriptableObject
    {
        [SerializeField] private Potion _prefab;
        [SerializeField] private AudioClip _sfx;

        public Potion Prefab => _prefab;
        public AudioClip SFX => _sfx;
    }
}