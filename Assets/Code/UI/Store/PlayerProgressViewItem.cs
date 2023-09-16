using Code.Infrastructure.Services.ProgressServices;
using TMPro;
using UnityEngine;

namespace Code.UI.Store
{
    public class PlayerProgressViewItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinsAmountText;
        [SerializeField] private TextMeshProUGUI _reputationAmountText;
        
        private IPersistentProgressService _progressService;

        public void Initialize(IPersistentProgressService progressService)
        {
            _progressService = progressService;

            _progressService.CoinsAmountChanged += OnCoinsAmountChanged;
            _progressService.ReputationAmountChanged += OnReputationAmountChanged;
            
            OnCoinsAmountChanged();
            OnReputationAmountChanged();
        }

        private void OnDestroy()
        {
            _progressService.CoinsAmountChanged -= OnCoinsAmountChanged;
            _progressService.ReputationAmountChanged -= OnReputationAmountChanged;
        }

        private void OnCoinsAmountChanged()
        {
            _coinsAmountText.text = _progressService.CoinsAmount.ToString();
        }

        private void OnReputationAmountChanged()
        {
            _reputationAmountText.text = _progressService.ReputationAmount.ToString();
        }
    }
}