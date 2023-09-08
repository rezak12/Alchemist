using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Infrastructure.Services.Factories;
using Code.Logic.PotionMaking;
using Code.StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.UI.PlayerIngredientUI
{
    public class IngredientItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Transform _characteristicInfoItemsContainer;
        [SerializeField] private Button _useButton;
        
        private IngredientData _ingredient;
        private AlchemyTable _alchemyTable;

        public async Task InitializeAsync(IngredientData ingredient, AlchemyTable alchemyTable, IUIFactory uiFactory)
        {
            _ingredient = ingredient;
            _alchemyTable = alchemyTable;
            
            _nameText.text = ingredient.Name;
            _iconImage.sprite = ingredient.Icon;
            
            await CreateIngredientCharacteristicItemsAsync(ingredient, uiFactory);

            _alchemyTable.FilledSlotsAmountChanged += FilledSlotAmountChanged;
            _useButton.onClick.AddListener(UseIngredient);
        }

        private void OnDestroy()
        {
            _alchemyTable.FilledSlotsAmountChanged -= FilledSlotAmountChanged;
            _useButton.onClick.RemoveListener(UseIngredient);
        }

        private async Task CreateIngredientCharacteristicItemsAsync(IngredientData ingredient, IUIFactory uiFactory)
        {
            var tasks = new List<Task>(ingredient.CharacteristicAmountPairs.Count);
            foreach (IngredientCharacteristicAmountPair characteristicAmountPair in ingredient.CharacteristicAmountPairs)
            {
                Task task = uiFactory.CreateIngredientCharacteristicItemUIAsync(
                    characteristicAmountPair, _characteristicInfoItemsContainer);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }

        private void FilledSlotAmountChanged()
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

        private void UseIngredient()
        {
            _alchemyTable.AddIngredient(_ingredient);
        }
    }
}