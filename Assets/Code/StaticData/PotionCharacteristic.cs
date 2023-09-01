using UnityEngine;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "CharacteristicData", menuName = "StaticData/PotionCharacteristics")]
    public class PotionCharacteristic : ScriptableObject
    {
        [SerializeField] private string _name;
        
        public string Name => _name;
    }
}