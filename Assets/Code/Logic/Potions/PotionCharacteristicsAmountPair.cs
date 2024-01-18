using Code.StaticData;
using Code.StaticData.Potions;

namespace Code.Logic.Potions
{
    public class PotionCharacteristicAmountPair
    {
        public PotionCharacteristic Characteristic { get; private set; }
        public int PointsAmount { get; private set; }

        public PotionCharacteristicAmountPair(PotionCharacteristic characteristic, int amount)
        {
            Characteristic = characteristic;
            PointsAmount = amount;
        }
    }
}