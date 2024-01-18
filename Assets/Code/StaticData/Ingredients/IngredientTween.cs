using DG.Tweening;
using UnityEngine;

namespace Code.StaticData.Ingredients
{
    [CreateAssetMenu(fileName = "NewIngredientTween", menuName = "StaticData/Tweens/Ingredient")]
    public class IngredientTween : ScriptableObject
    {
        [field: SerializeField] public float JumpDurationInSeconds { get; private set; } = 1f;
        [field: SerializeField] public float JumpPower { get; private set; } = 2;
        [field: SerializeField] public Ease JumpingEase { get; private set; }
    }
}