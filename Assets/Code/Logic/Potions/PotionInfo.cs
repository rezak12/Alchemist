using System.Collections.Generic;

namespace Code.Logic.Potions
{
    public class PotionInfo
    {
        public List<PotionCharacteristicAmountPair> Characteristics { get; }

        public PotionInfo(List<PotionCharacteristicAmountPair> characteristics)
        {
            Characteristics = characteristics;
        }
    }
}