using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Animations
{
    public class IngredientAnimator : MonoBehaviour
    {
        [SerializeField] private float _removeDurationInSeconds = 10f;

        public async UniTaskVoid MoveToSlot(Transform slotTransform)
        {
            await transform.DOMove(slotTransform.position, 10f, false)
                .WithCancellation(this.GetCancellationTokenOnDestroy());
        }

        public async UniTaskVoid RemoveFromSlotThenDestroy(Transform to, Action onRemoved = null)
        {
            await transform.DOJump(to.position, 5f, 1, _removeDurationInSeconds, false)
                .WithCancellation(this.GetCancellationTokenOnDestroy());
            
            onRemoved?.Invoke();
            Destroy(gameObject);
        }
    }
}