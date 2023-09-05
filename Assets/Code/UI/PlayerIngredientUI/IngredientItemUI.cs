using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Infrastructure.Services.Factories;
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

        public async Task InitializeAsync
            (string ingredientName, 
            Sprite ingredientIcon, 
            List<IngredientCharacteristicAmountPair> characteristicAmountPairs, 
            Action onUseButtonClickCallback, 
            IUIFactory uiFactory)
        {
            _nameText.text = ingredientName;
            _iconImage.sprite = ingredientIcon;

            var tasks = new List<Task>(characteristicAmountPairs.Count);
            foreach (IngredientCharacteristicAmountPair characteristicAmountPair in characteristicAmountPairs)
            { 
                Task task = uiFactory.CreateIngredientCharacteristicItemUIAsync(
                    characteristicAmountPair, _characteristicInfoItemsContainer);
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
            
            _useButton.onClick.AddListener(new UnityAction(onUseButtonClickCallback));
        }
    }
}