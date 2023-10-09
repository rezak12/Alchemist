using TMPro;
using UnityEngine;

namespace Code.UI.AwaitingOverlays
{
    public class AwaitingOverlay : MonoBehaviour, IAwaitingOverlay
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TextMeshProUGUI _messageText;

        private void Awake()
        {
            Hide();
        }

        public void Show(string message = "")
        {
            _messageText.text = message;
            _canvas.enabled = true;
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }
    }
}