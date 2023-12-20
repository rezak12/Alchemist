using Code.StaticData;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Animations
{
    public class IngredientTweener : MonoBehaviour
    {
        [SerializeField] private IngredientTween _tween;

        public async UniTask JumpTo(Transform to)
        {
            await transform
                .DOJump(to.position, _tween.JumpPower, 1, _tween.JumpDurationInSeconds)
                .SetEase(_tween.JumpingEase)
                .WithCancellation(this.GetCancellationTokenOnDestroy());
        }
    }
}