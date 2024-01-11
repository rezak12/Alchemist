using UnityEngine;

namespace Code.StaticData.Potions
{
    [CreateAssetMenu(fileName = "CharacteristicData", menuName = "StaticData/PotionCharacteristic")]
    public class PotionCharacteristic : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;
        
        public string Name => _name;
        public Sprite Icon => _icon;
    }
}