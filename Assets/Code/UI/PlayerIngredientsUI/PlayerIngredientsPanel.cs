using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Infrastructure.Services.Factories;
using Code.Logic.PotionMaking;
using Code.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.UI.PlayerIngredientsUI
{
    public class PlayerIngredientsPanel : MonoBehaviour
    {
        [SerializeField] private Transform _ingredientItemsContainer;
        
        public async UniTask InitializeAsync(IEnumerable<IngredientData> playerIngredients, AlchemyTable alchemyTable, IUIFactory uiFactory)
        {
            var tasks = new List<UniTask>();
            foreach (IngredientData ingredient in playerIngredients)
            {
                var task = uiFactory.CreateIngredientItemUIAsync(ingredient, alchemyTable, _ingredientItemsContainer);
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
        }
    }
}