using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Infrastructure.Services.Factories;
using Code.Logic.PotionMaking;
using Code.StaticData;
using UnityEngine;

namespace Code.UI.PlayerIngredientsUI
{
    public class PlayerIngredientsPanel : MonoBehaviour
    {
        [SerializeField] private Transform _ingredientItemsContainer;
        
        public async Task InitializeAsync(IEnumerable<IngredientData> playerIngredients, AlchemyTable alchemyTable, IUIFactory uiFactory)
        {
            var tasks = new List<Task>();
            foreach (IngredientData ingredient in playerIngredients)
            {
                var task = uiFactory.CreateIngredientItemUIAsync(ingredient, alchemyTable, _ingredientItemsContainer);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }
    }
}