using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

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

        public async UniTask RemoveFromSlotThenDestroy(Transform to, Action onRemoved = null)
        {
            await transform.DOJump(to.position, _jumpPowerWhileRemoving, 1, _moveDurationInSeconds, false)
                .WithCancellation(this.GetCancellationTokenOnDestroy());
            
            onRemoved?.Invoke();
            Destroy(gameObject);
        }
    }
}