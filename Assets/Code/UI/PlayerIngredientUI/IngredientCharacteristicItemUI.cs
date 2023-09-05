using Code.StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.PlayerIngredientUI
{
    public class IngredientCharacteristicItemUI : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _pointsAmountText;

        public void Initialize(Sprite ingredientIcon, int characteristicPointAmount)
        {
            _iconImage.sprite = ingredientIcon;
            _pointsAmountText.text = characteristicPointAmount.ToString();
        }
    }
}