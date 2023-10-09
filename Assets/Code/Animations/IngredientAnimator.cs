using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Animations
{
    public class IngredientAnimator : MonoBehaviour
    {
        [SerializeField] private float _moveDurationInSeconds = 1f;
        [SerializeField] private float _jumpPowerWhileRemoving = 2;

        public async UniTask MoveToSlot(Transform slotTransform)
        {
            await transform.DOMove(slotTransform.position, _moveDurationInSeconds, false)
                .WithCancellation(this.GetCancellationTokenOnDestroy());
        }

        public async UniTask RemoveFromSlot(Transform to)
        {
            await transform.DOJump(to.position, _jumpPowerWhileRemoving, 1, _moveDurationInSeconds, false)
                .WithCancellation(this.GetCancellationTokenOnDestroy());
        }
    }
}