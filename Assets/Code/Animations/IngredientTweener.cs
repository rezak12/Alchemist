using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Animations
{
    public class IngredientTweener : MonoBehaviour
    {
        [SerializeField] private float _jumpDurationInSeconds = 1f;
        [SerializeField] private float _jumpPower = 2;
        [SerializeField] private Ease _jumpingEase;

        public async UniTask JumpTo(Transform to)
        {
            await transform
                .DOJump(to.position, _jumpPower, 1, _jumpDurationInSeconds)
                .SetEase(_jumpingEase)
                .WithCancellation(this.GetCancellationTokenOnDestroy());
        }
    }
}