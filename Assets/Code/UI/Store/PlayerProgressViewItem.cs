using Code.Infrastructure.Services.ProgressServices;
using TMPro;
using UnityEngine;
using Zenject;

namespace Code.UI.Store
{
    public class PlayerProgressViewItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinsAmountText;
        [SerializeField] private TextMeshProUGUI _reputationAmountText;
        
        private IPersistentProgressService _progressService;

        [Inject]
        private void Construct(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }
        
        private void Start()
        {
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