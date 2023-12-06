using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Code.Animations
{
    public class PotionTweener : MonoBehaviour
    {
        [Header("Move Up")] 
        [SerializeField] private float _moveUpPoints;
        [SerializeField] private float _moveUpDuration;
        [SerializeField] private float _oneRotateDuration;
        [SerializeField] private Ease _rotationEase;

        [Header("Move To Camera")]
        [SerializeField] private float _moveToCameraDuration;
        [SerializeField] private float _cameraZOffset;
        [SerializeField] private Ease _moveToCameraEase;

        [Header("Scale")]
        [SerializeField] private float _scaleDuration;
        [SerializeField] private Ease _scaleEase;

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
            var positionToMove = new Vector3(transformPosition.x, transformPosition.y + _moveUpPoints, transformPosition.z);

            using var rotationCancelSource = new CancellationTokenSource();
            CancellationToken cancellationToken = rotationCancelSource.Token;

            await UniTask.WhenAny(
                transform
                    .DOMove(positionToMove, _moveUpDuration)
                    .WithCancellation(this.GetCancellationTokenOnDestroy()),

                transform
                    .DORotate(new Vector3(0, 360, 0), _oneRotateDuration, RotateMode.FastBeyond360)
                    .SetEase(_rotationEase)
                    .SetLoops(-1)
                    .WithCancellation(cancellationToken));
            
            rotationCancelSource.Cancel();
        }

        private async UniTask MoveToCamera()
        {
            Vector3 cameraPosition = _camera.transform.position;
            Quaternion cameraRotation = _camera.transform.rotation;
            Vector3 cameraForward = cameraRotation * Vector3.forward;
            Vector3 positionToMove = cameraPosition + cameraForward * _cameraZOffset;
            
            Vector3 directionToCamera = cameraPosition - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);

            await UniTask.WhenAll(
                transform.DOMove(positionToMove, _moveToCameraDuration)
                    .SetEase(_moveToCameraEase)
                    .WithCancellation(this.GetCancellationTokenOnDestroy()),
                
                transform.DORotateQuaternion(targetRotation, _moveToCameraDuration)
                    .SetEase(_moveToCameraEase)
                    .WithCancellation(this.GetCancellationTokenOnDestroy()));
        }

        private async UniTask ScaleToZero()
        {
            await transform
                .DOScale(Vector3.zero, _scaleDuration)
                .SetEase(_scaleEase)
                .WithCancellation(this.GetCancellationTokenOnDestroy()); 
        }
    }
}