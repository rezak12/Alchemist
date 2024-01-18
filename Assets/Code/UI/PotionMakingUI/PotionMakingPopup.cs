using System.Collections.Generic;
using Code.Logic.PotionMaking;
using Code.StaticData;
using Code.StaticData.Ingredients;
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
            AlchemyTableComponent alchemyTable)
        {
            await _ingredientsPanel.InitializeAsync(playerIngredients, alchemyTable);
            _alchemyTableInteractionPanel.Initialize(alchemyTable);
        }
    }
}