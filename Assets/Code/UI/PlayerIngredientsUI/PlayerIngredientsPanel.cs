using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Infrastructure.Services.Factories;
using Code.Logic.PotionMaking;
using Code.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.UI.PlayerIngredientsUI
{
    public class PlayerIngredientsPanel : MonoBehaviour
    {
        [SerializeField] private Transform _ingredientItemsContainer;
        
        private IUIFactory _uiFactory;

        [Inject]
        private void Construct(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }
        
        public async UniTask InitializeAsync(
            IEnumerable<IngredientData> playerIngredients, 
            AlchemyTable alchemyTable)
        {
            var tasks = new List<UniTask>();
            foreach (IngredientData ingredient in playerIngredients)
            {
                var task = _uiFactory.CreateIngredientItemUIAsync(ingredient, alchemyTable, _ingredientItemsContainer);
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
        }
    }
}