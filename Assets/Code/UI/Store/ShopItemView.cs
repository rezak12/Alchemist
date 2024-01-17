using Code.Infrastructure.Services.ProgressServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.Store
{
    public class ShopItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _requiredCoinsText;
        [SerializeField] private TextMeshProUGUI _requiredReputationText;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _selectButton;
        [SerializeField] private Image _lockImage;
        [SerializeField] private Image _selectLabel;

        public bool IsSelected => _selectLabel.gameObject.activeSelf;

        private int _requiredCoinsAmount;
        private int _requiredReputationAmount;

        private IPersistentProgressService _progressService;

        [Inject]
        private void Construct(IPersistentProgressService progressService) => _progressService = progressService;

        private void OnEnable()
        {
            _progressService.CoinsAmountChanged += OnPlayerProgressChanged;
            _progressService.ReputationAmountChanged += OnPlayerProgressChanged;
        }

        private void OnDisable()
        {
            _progressService.CoinsAmountChanged -= OnPlayerProgressChanged;
            _progressService.ReputationAmountChanged -= OnPlayerProgressChanged;
            _buyButton.onClick.RemoveAllListeners();
            _selectButton.onClick.RemoveAllListeners();
        }

        public void SetItem(
            string itemName, 
            Sprite itemIcon, 
            int requiredCoinsAmount, 
            int requiredReputationAmount, 
            UnityAction onBuyButtonClicked,
            UnityAction onSelectButtonClicked)
        {
            _itemNameText.text = itemName;
            _itemIcon.sprite = itemIcon;
            _requiredCoinsText.text = requiredCoinsAmount.ToString();
            _requiredReputationText.text = requiredReputationAmount.ToString();
            
            _buyButton.onClick.AddListener(onBuyButtonClicked);
            _buyButton.onClick.AddListener(Unlock);
            
            _selectButton.onClick.AddListener(onSelectButtonClicked);
            _selectButton.onClick.AddListener(Select);

            _requiredCoinsAmount = requiredCoinsAmount;
            _requiredReputationAmount = requiredReputationAmount;
            
            OnPlayerProgressChanged();
        }

        public void Lock()
        {
            _lockImage.gameObject.SetActive(true);
            _buyButton.gameObject.SetActive(true);
            _selectButton.gameObject.SetActive(false);
        }

        public void Unlock()
        {
            _lockImage.gameObject.SetActive(false);
            _buyButton.gameObject.SetActive(false);
            _selectButton.gameObject.SetActive(true);
        }

        public void Select()
        {
            _selectButton.gameObject.SetActive(false);
            _selectLabel.gameObject.SetActive(true);
        }

        public void Unselect()
        {
            _selectButton.gameObject.SetActive(true);
            _selectLabel.gameObject.SetActive(false);
        }

        private void OnPlayerProgressChanged()
        {
            bool isCoinsEnough = _progressService.IsCoinsEnoughFor(_requiredCoinsAmount);
            bool isReputationEnough = _progressService.IsReputationEnoughFor(_requiredReputationAmount);

            _buyButton.interactable = isCoinsEnough && isReputationEnough;
        }
    }
}