using UnityEngine;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "CharacteristicData", menuName = "StaticData/PotionCharacteristics")]
    public class PotionCharacteristic : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;
        
        public string Name => _name;
        public Sprite Icon => _icon;
    }
}