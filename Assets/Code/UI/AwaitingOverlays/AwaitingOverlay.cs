using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.AwaitingOverlays
{
    public class AwaitingOverlay : MonoBehaviour, IAwaitingOverlay
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private float _alphaMovingDuration;

        private void Awake()
        {
            Hide().Forget();
        }

        public async UniTask Show(string message = "")
        {
            _canvas.enabled = true;
            _messageText.text = message;
            await MoveAlphaTo(1);
        }

        public async UniTask Hide()
        {
            await MoveAlphaTo(0);
            _canvas.enabled = false;
        }

        private async UniTask MoveAlphaTo(float value)
        {
            Color backgroundFinalColor = _backgroundImage.color;
            backgroundFinalColor.a = value;

            Color textFinalColor = _messageText.color;
            textFinalColor.a = value;

            await UniTask.WhenAll(
                _backgroundImage.DOColor(backgroundFinalColor, _alphaMovingDuration)
                    .WithCancellation(this.GetCancellationTokenOnDestroy()),
                _messageText.DOColor(textFinalColor, _alphaMovingDuration)
                    .WithCancellation(this.GetCancellationTokenOnDestroy())
            );
        }
        
        public class Factory : PlaceholderFactory<string, UniTask<AwaitingOverlay>> { }
    }
}