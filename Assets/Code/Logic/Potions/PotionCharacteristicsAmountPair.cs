using Code.StaticData;

namespace Code.Logic.Potions
{
    public class PotionCharacteristicAmountPair
    {
        public PotionCharacteristic Characteristic { get; private set; }
        public int Amount { get; private set; }

        public PotionCharacteristicAmountPair(PotionCharacteristic characteristic, int amount)
        {
            Characteristic = characteristic;
            Amount = amount;
        }
    }
}