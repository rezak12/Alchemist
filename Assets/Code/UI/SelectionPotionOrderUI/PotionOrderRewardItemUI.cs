using Code.Infrastructure.Services.AssetProvider;
using Code.Logic.Orders;
using Code.StaticData;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.SelectionPotionOrderUI
{
    public class PotionOrderRewardItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinsAmountText;
        [SerializeField] private TextMeshProUGUI _reputationAmountText;
        [Space]
        [SerializeField] private GameObject _ingredientContainer;
        [SerializeField] private Image _ingredientIcon;
        [SerializeField] private TextMeshProUGUI _ingredientNameText;
        
        private IAssetProvider _assetProvider;

        [Inject]
        private void Construct(IAssetProvider assetProvider) => _assetProvider = assetProvider;

        public async UniTaskVoid SetReward(PotionOrderReward reward)
        {
            _coinsAmountText.text = reward.CoinsAmount.ToString();
            _reputationAmountText.text = reward.ReputationAmount.ToString();

            if (reward.IngredientReference == null)
            {
                _ingredientContainer.SetActive(false);
            }
            else
            {
                var ingredient = await _assetProvider.LoadAsync<IngredientData>(reward.IngredientReference);
                
                _ingredientIcon.sprite = ingredient.Icon;
                _ingredientNameText.text = ingredient.Name;
                
                _ingredientContainer.SetActive(true);
            }
        }
    }
}