using System.Collections.Generic;
using Code.Infrastructure.Services.Factories;
using Code.Logic.PotionMaking;
using Code.StaticData;
using Code.UI.PlayerIngredientsUI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.UI.PotionMakingUI
{
    public class PotionMakingPopup : MonoBehaviour
    {
        [SerializeField] private PlayerIngredientsPanel _ingredientsPanel;
        [SerializeField] private AlchemyTableInteractionPanel _alchemyTableInteractionPanel;

        public async UniTask InitializeAsync(
            IEnumerable<IngredientData> playerIngredients, 
            AlchemyTable alchemyTable,
            IUIFactory uiFactory)
        {
            await _ingredientsPanel.InitializeAsync(playerIngredients, alchemyTable, uiFactory);
            _alchemyTableInteractionPanel.Initialize(alchemyTable);
        }
    }
}