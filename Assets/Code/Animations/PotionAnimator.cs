using DG.Tweening;
using UnityEngine;

namespace Code.Animations
{
    public class PotionAnimator : MonoBehaviour
    {
        [SerializeField] private float _moveUpPoints;

        public void PresentAfterCreating()
        {
            Vector3 transformPosition = transform.position;
            _moveUpPoints = 5f;
            var posToMove = new Vector3(transformPosition.x, transformPosition.y + _moveUpPoints, transformPosition.z);
            transform.DORotate(posToMove, -1f, RotateMode.Fast);
        }
    }
}