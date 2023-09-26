using System.Collections.Generic;
using Code.Infrastructure.Services.Factories;
using Code.Logic.Potions;
using Code.StaticData;
using Code.UI.PlayerIngredientsUI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.UI.PotionCharacteristicsUI
{
    public class PotionCharacteristicItemsContainer : MonoBehaviour
    {
        private readonly List<PotionCharacteristicItemUI> _items = new();
        public async UniTask CreateCharacteristicItemsAsync(
            IEnumerable<IngredientCharacteristicAmountPair> characteristicAmountPairs,
            IUIFactory uiFactory)
        {
            Cleanup();
            
            var tasks = new List<UniTask>();
            foreach (IngredientCharacteristicAmountPair characteristicAmountPair in characteristicAmountPairs)
            {
                UniTask task = uiFactory.CreatePotionCharacteristicItemUIAsync(
                    characteristicAmountPair, transform).ContinueWith(item => _items.Add(item));
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask CreateCharacteristicItemsAsync(
            IEnumerable<PotionCharacteristicAmountPair> characteristicAmountPairs,
            IUIFactory uiFactory)
        {
            var tasks = new List<UniTask>();
            foreach (PotionCharacteristicAmountPair characteristicAmountPair in characteristicAmountPairs)
            {
                UniTask task = uiFactory.CreatePotionCharacteristicItemUIAsync(
                    characteristicAmountPair, transform).ContinueWith(item => _items.Add(item));
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
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