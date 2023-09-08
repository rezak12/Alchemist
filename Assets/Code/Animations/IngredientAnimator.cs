using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Code.Animations
{
    public class IngredientAnimator : MonoBehaviour
    {
        [SerializeField] private float _removeDurationInSeconds = 10f;

        public void MoveToSlot(Transform slotTransform)
        {
            transform.DOMove(slotTransform.position, 10f, false);
        }

        public void RemoveFromSlot(Transform to, Action onRemoved = null)
        {
            if (onRemoved != null)
            {
                StartCoroutine(DoAfterRemoveFromSlot(onRemoved));
            }
            transform.DOJump(to.position, 5f, 1, _removeDurationInSeconds, false);
        }

        private IEnumerator DoAfterRemoveFromSlot(Action onRemoved)
        {
            yield return new WaitForSeconds(_removeDurationInSeconds);
            onRemoved.Invoke();
        }
    }
}