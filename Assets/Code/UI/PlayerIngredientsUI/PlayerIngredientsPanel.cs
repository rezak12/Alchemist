using System.Collections.Generic;
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
        private void Construct(IUIFactory uiFactory) => _uiFactory = uiFactory;

        public async UniTask InitializeAsync(
            IEnumerable<IngredientData> playerIngredients, 
            AlchemyTableComponent alchemyTable)
        {
            var tasks = new List<UniTask>();
            foreach (IngredientData ingredient in playerIngredients)
            {
                tasks.Add(_uiFactory.CreateIngredientItemUIAsync(ingredient, alchemyTable, _ingredientItemsContainer));
            }

            await UniTask.WhenAll(tasks);
        }
    }
}