using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Infrastructure.Services.Factories;
using Code.Logic.Potions;
using Code.StaticData;
using Code.UI.PlayerIngredientsUI;
using UnityEngine;

namespace Code.UI.PotionCharacteristicsUI
{
    public class PotionCharacteristicItemsContainer : MonoBehaviour
    {
        private readonly List<PotionCharacteristicItemUI> _items = new();
        public async Task CreateCharacteristicItemsAsync(
            List<IngredientCharacteristicAmountPair> characteristicAmountPairs,
            IUIFactory uiFactory)
        {
            Cleanup();
            
            var tasks = new List<Task<PotionCharacteristicItemUI>>(characteristicAmountPairs.Count);
            foreach (IngredientCharacteristicAmountPair characteristicAmountPair in characteristicAmountPairs)
            {
                var task = uiFactory.CreatePotionCharacteristicItemUIAsync(
                    characteristicAmountPair, transform);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                _items.Add(task.Result);
            }
        }

        public async Task CreateCharacteristicItemsAsync(
            List<PotionCharacteristicAmountPair> characteristicAmountPairs,
            IUIFactory uiFactory)
        {
            var tasks = new List<Task>(characteristicAmountPairs.Count);
            foreach (PotionCharacteristicAmountPair characteristicAmountPair in characteristicAmountPairs)
            {
                Task task = uiFactory.CreatePotionCharacteristicItemUIAsync(
                    characteristicAmountPair, transform);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }

        private void Cleanup()
        {
            foreach (PotionCharacteristicItemUI item in _items)
            {
                Destroy(item.gameObject);
            }
            _items.Clear();
        }
    }
}