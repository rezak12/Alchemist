using Code.Logic.PotionMaking;
using Code.StaticData;
using Code.StaticData.Ingredients;
using Code.UI.PotionCharacteristicsUI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.PlayerIngredientsUI
{
    public class IngredientItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private PotionCharacteristicItemsContainer _characteristicItemsContainer;
        [SerializeField] private Button _useButton;
        
        private IngredientData _ingredient;
        private AlchemyTableComponent _alchemyTable;

        public async UniTask InitializeAsync(IngredientData ingredient, AlchemyTableComponent alchemyTable)
        {
            _ingredient = ingredient;
            _alchemyTable = alchemyTable;
            
            _nameText.text = ingredient.Name;
            _iconImage.sprite = ingredient.Icon;
            
            await _characteristicItemsContainer.CreateCharacteristicItemsAsync(ingredient.CharacteristicAmountPairs);

            _alchemyTable.FilledSlotsAmountChanged += OnFilledSlotAmountChanged;
            _useButton.onClick.AddListener(UseIngredient);
        }

        private void OnDestroy()
        {
            _alchemyTable.FilledSlotsAmountChanged -= OnFilledSlotAmountChanged;
            _useButton.onClick.RemoveListener(UseIngredient);
        }

        private void OnFilledSlotAmountChanged()
        {
            if (_alchemyTable.IsAllSlotsFilled)
            {
                _useButton.interactable = false;
            }
            else if (!_useButton.interactable)
            {
                _useButton.interactable = true;
            }
        }

        private void UseIngredient() => _alchemyTable.AddIngredient(_ingredient);
    }
}