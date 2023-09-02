using Code.StaticData;
using UnityEngine;

namespace Code.Logic.PotionMaking
{
    public class AlchemyTableSlot : MonoBehaviour
    {
        public bool IsFilled => CurrentIngredient != null;

        public IngredientData CurrentIngredient { get; private set; }

        public void Fill(IngredientData ingredient)
        {
            CurrentIngredient = ingredient;
        }

        public void Release()
        {
            CurrentIngredient = null;
        }
    }
}