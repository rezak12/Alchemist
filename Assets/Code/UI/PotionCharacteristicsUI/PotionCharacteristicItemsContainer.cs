using System.Collections.Generic;
using Code.Infrastructure.Services.Factories;
using Code.Logic.Potions;
using Code.StaticData;
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
        private void Construct(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }
        
        public async UniTask CreateCharacteristicItemsAsync(
            IEnumerable<IngredientCharacteristicAmountPair> characteristicAmountPairs)
        {
            Cleanup();
            
            var tasks = new List<UniTask>();
            foreach (IngredientCharacteristicAmountPair characteristicAmountPair in characteristicAmountPairs)
            {
                UniTask task = _uiFactory.CreatePotionCharacteristicItemUIAsync(
                    characteristicAmountPair, transform).ContinueWith(item => _items.Add(item));
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask CreateCharacteristicItemsAsync(
            IEnumerable<PotionCharacteristicAmountPair> characteristicAmountPairs)
        {
            var tasks = new List<UniTask>();
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