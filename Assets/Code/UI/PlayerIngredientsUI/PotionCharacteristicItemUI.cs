using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.PlayerIngredientsUI
{
    public class PotionCharacteristicItemUI : MonoBehaviour
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