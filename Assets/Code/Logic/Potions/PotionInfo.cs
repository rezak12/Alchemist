using System.Collections.Generic;

namespace Code.Logic.Potions
{
    public class PotionInfo
    {
        public PotionCharacteristicAmountPair[] Characteristics { get; }

        public PotionInfo(PotionCharacteristicAmountPair[] characteristics)
        {
            Characteristics = characteristics;
        }
    }
}