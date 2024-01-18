using System.Collections.Generic;

namespace Code.Logic.Potions
{
    public class PotionInfo
    {
        public IEnumerable<PotionCharacteristicAmountPair> CharacteristicsAmountPairs { get; }

        public PotionInfo(IEnumerable<PotionCharacteristicAmountPair> characteristicsAmountPairs) => 
            CharacteristicsAmountPairs = characteristicsAmountPairs;
    }
}