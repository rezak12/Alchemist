using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Animations
{
    public class PotionAnimator : MonoBehaviour
    {
        [SerializeField] private float _moveUpPoints = 5f;

        public async UniTask PresentAfterCreating()
        {
            Vector3 transformPosition = transform.position;
            var posToMove = new Vector3(transformPosition.x, transformPosition.y + _moveUpPoints, transformPosition.z);
            
            await transform.DORotate(posToMove, -1f, RotateMode.Fast)
                .WithCancellation(this.GetCancellationTokenOnDestroy());
        }
    }
}