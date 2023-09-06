using System.Collections.Generic;

namespace Code.Logic.Potions
{
    public class PotionInfo
    {
        public PotionCharacteristicAmountPair[] CharacteristicsAmountPairs { get; }

        public PotionInfo(PotionCharacteristicAmountPair[] characteristicsAmountPairs)
        {
            CharacteristicsAmountPairs = characteristicsAmountPairs;
        }
    }
}