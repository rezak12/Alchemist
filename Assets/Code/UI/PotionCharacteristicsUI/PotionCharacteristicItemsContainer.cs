using System.Collections.Generic;
using Code.Infrastructure.Services.Factories;
using Code.Logic.Potions;
using Code.StaticData;
using Code.StaticData.Ingredients;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.UI.PotionCharacteristicsUI
{
    public class PotionCharacteristicItemsContainer : MonoBehaviour
    {
        private readonly List<PotionCharacteristicItemUI> _items = new();
        
        private IUIFactory _uiFactory;

        [Inject]
        private void Construct(IUIFactory uiFactory) => _uiFactory = uiFactory;

        public async UniTask CreateCharacteristicItemsAsync(
            IReadOnlyCollection<IngredientCharacteristicAmountPair> characteristicAmountPairs)
        {
            Cleanup();
            
            var tasks = new List<UniTask>(characteristicAmountPairs.Count);
            foreach (IngredientCharacteristicAmountPair characteristicAmountPair in characteristicAmountPairs)
            {
                UniTask task = _uiFactory.CreatePotionCharacteristicItemUIAsync(
                    characteristicAmountPair, transform).ContinueWith(item => _items.Add(item));
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask CreateCharacteristicItemsAsync(
            IReadOnlyCollection<PotionCharacteristicAmountPair> characteristicAmountPairs)
        {
            Cleanup();
            
            var tasks = new List<UniTask>(characteristicAmountPairs.Count);
            foreach (PotionCharacteristicAmountPair characteristicAmountPair in characteristicAmountPairs)
            {
                UniTask task = _uiFactory.CreatePotionCharacteristicItemUIAsync(
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