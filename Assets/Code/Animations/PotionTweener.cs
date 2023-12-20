using System.Threading;
using Code.StaticData;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Code.Animations
{
    public class PotionTweener : MonoBehaviour
    {
        [SerializeField] private PotionTween _tween;
        private Camera _camera;

        [Inject]
        private void Construct(Camera camera) => _camera = camera;

        public async UniTask PresentAfterCreating()
        {
            await MoveUp();
            await MoveToCamera();
            await ScaleToZero();
        }

        private async UniTask MoveUp()
        {
            Vector3 transformPosition = transform.position;
            var positionToMove = new Vector3(
                transformPosition.x, 
                transformPosition.y + _tween.MoveUpPoints, 
                transformPosition.z);

            using var rotationCancelSource = new CancellationTokenSource();
            CancellationToken cancellationToken = rotationCancelSource.Token;

            await UniTask.WhenAny(
                transform
                    .DOMove(positionToMove, _tween.MoveUpDuration)
                    .WithCancellation(this.GetCancellationTokenOnDestroy()),

                transform
                    .DORotate(new Vector3(0, 360, 0), _tween.OneRotateDuration, RotateMode.FastBeyond360)
                    .SetEase(_tween.RotationEase)
                    .SetLoops(-1)
                    .WithCancellation(cancellationToken));
            
            rotationCancelSource.Cancel();
        }

        private async UniTask MoveToCamera()
        {
            Vector3 cameraPosition = _camera.transform.position;
            Quaternion cameraRotation = _camera.transform.rotation;
            Vector3 cameraForward = cameraRotation * Vector3.forward;
            Vector3 positionToMove = cameraPosition + cameraForward * _tween.CameraZOffset;
            
            Vector3 directionToCamera = cameraPosition - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);

            await UniTask.WhenAll(
                transform.DOMove(positionToMove, _tween.MoveToCameraDuration)
                    .SetEase(_tween.MoveToCameraEase)
                    .WithCancellation(this.GetCancellationTokenOnDestroy()),
                
                transform.DORotateQuaternion(targetRotation, _tween.MoveToCameraDuration)
                    .SetEase(_tween.MoveToCameraEase)
                    .WithCancellation(this.GetCancellationTokenOnDestroy()));
        }

        private async UniTask ScaleToZero()
        {
            await transform
                .DOScale(Vector3.zero, _tween.ScaleDuration)
                .SetEase(_tween.ScaleEase)
                .WithCancellation(this.GetCancellationTokenOnDestroy()); 
        }
    }
}