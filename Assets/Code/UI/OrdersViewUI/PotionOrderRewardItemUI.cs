using Code.Logic.Orders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.OrdersViewUI
{
    public class PotionOrderRewardItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinsAmountText;
        [SerializeField] private TextMeshProUGUI _reputationAmountText;
        [Space]
        [SerializeField] private GameObject _ingredientContainer;
        [SerializeField] private Image _ingredientIcon;
        [SerializeField] private TextMeshProUGUI _ingredientNameText;

        public void SetReward(PotionOrderReward reward)
        {
            _coinsAmountText.text = reward.CoinsAmount.ToString();
            _reputationAmountText.text = reward.ReputationAmount.ToString();

            if (reward.Ingredient == null)
            {
                _ingredientContainer.SetActive(false);
            }
            else
            {
                _ingredientIcon.sprite = reward.Ingredient.Icon;
                _ingredientNameText.text = reward.Ingredient.Name;
                
                _ingredientContainer.SetActive(true);
            }
        }
    }
}