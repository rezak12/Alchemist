using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Animations;
using Code.Infrastructure.Services.Factories;
using Code.Logic.Potions;
using Code.StaticData;
using UnityEngine;
using Zenject;

namespace Code.Logic.PotionMaking
{
    public class AlchemyTable : MonoBehaviour
    {
        [SerializeField] private AlchemyTableSlot[] _tableSlots;
        [SerializeField] private Transform _ingredientsSpawnPoint;
        [SerializeField] private Transform _ingredientsRemoveFromSlotPoint;
        [SerializeField] private Transform _potionSpawnPoint;
        
        public event Action FilledSlotsAmountChanged;
        public bool IsAllSlotsFilled => _freeSlots.Count < 1;
        public bool IsAllSlotsFree => _filledSlots.Count < 1;
        
        private Stack<AlchemyTableSlot> _freeSlots;
        private Stack<AlchemyTableSlot> _filledSlots;
        private Stack<IngredientAnimator> _ingredientsAnimators;
        
        private IPotionInfoFactory _potionInfoFactory;
        private IPotionFactory _potionFactory;
        private IIngredientFactory _ingredientFactory;

        [Inject]
        private void Construct(
            IPotionInfoFactory potionInfoFactory, 
            IIngredientFactory ingredientFactory,
            IPotionFactory potionFactory)
        {
            _potionFactory = potionFactory;
            _potionInfoFactory = potionInfoFactory;
            _ingredientFactory = ingredientFactory;
        }

        public void Initialize()
        {
            InitializeSlotsCollections();
        }

        public void AddIngredient(IngredientData ingredient)
        {
            FillSlot(ingredient);
            StartCoroutine(MoveNewIngredientToSlot(ingredient, _filledSlots.Peek().transform));
        }

        public void RemoveLastIngredient()
        {
            ReleaseLastSlot();
            RemoveLastIngredientPrefabFromSlot();
        }

        public void HandleResult()
        {
            StartCoroutine(HandleResultCoroutine());
        }

        private IEnumerator HandleResultCoroutine()
        {
            var ingredients = TakeAllIngredients();
            MoveAllIngredientsToPotionCreatingPoint();
            
            var task = CreatePotion(ingredients);
            yield return task;
            Potion potion = task.Result;

            var potionAnimator = potion.GetComponent<PotionAnimator>();
            potionAnimator.PresentAfterCreating();
            
            Cleanup();
            
        }

        private async Task<Potion> CreatePotion(IEnumerable<IngredientData> ingredients)
        {
            PotionInfo potionInfo = await _potionInfoFactory.CreatePotionInfoAsync(ingredients);
            Potion potion = await _potionFactory.CreatePotionAsync(potionInfo, _potionSpawnPoint.position);
            
            return potion;
        }

        private void FillSlot(IngredientData ingredient)
        {
            AlchemyTableSlot slot = _freeSlots.Pop();
            
            slot.Fill(ingredient);
            _filledSlots.Push(slot);
            
            FilledSlotsAmountChanged?.Invoke();
        }

        private void ReleaseLastSlot()
        {
            AlchemyTableSlot slot = _filledSlots.Pop();
            
            slot.Release();
            _freeSlots.Push(slot);
            
            FilledSlotsAmountChanged?.Invoke();
        }

        private IEnumerable<IngredientData> TakeAllIngredients()
        {
            var ingredients = new List<IngredientData>(_filledSlots.Count);

            foreach (AlchemyTableSlot slot in _filledSlots)
            {
                ingredients.Add(slot.CurrentIngredient);
                slot.Release();
            }

            return ingredients;
        }

        private IEnumerator MoveNewIngredientToSlot(IngredientData ingredientData, Transform slotTransform)
        {
            var task = _ingredientFactory.CreateIngredientAsync(
                ingredientData.PrefabReference, _ingredientsSpawnPoint.position);
            
            yield return task;

            IngredientAnimator ingredientAnimator = task.Result;
            _ingredientsAnimators.Push(ingredientAnimator);
            
            ingredientAnimator.MoveToSlot(slotTransform);
        }

        private void MoveAllIngredientsToPotionCreatingPoint()
        {
            foreach (IngredientAnimator ingredientAnimator in _ingredientsAnimators)
            {
                RemoveIngredientFromSlotThenDestroy(ingredientAnimator, _potionSpawnPoint);
            }
        }

        private void RemoveLastIngredientPrefabFromSlot()
        {
            IngredientAnimator ingredientAnimator = _ingredientsAnimators.Pop();
            RemoveIngredientFromSlotThenDestroy(ingredientAnimator, _ingredientsRemoveFromSlotPoint);
        }

        private void RemoveIngredientFromSlotThenDestroy(IngredientAnimator ingredientAnimator, Transform to)
        {
            ingredientAnimator.RemoveFromSlot(to, () => Destroy(ingredientAnimator.gameObject));
        }

        private void InitializeSlotsCollections()
        {
            _freeSlots = new Stack<AlchemyTableSlot>(_tableSlots);
            _filledSlots = new Stack<AlchemyTableSlot>(_freeSlots.Count);
            _ingredientsAnimators = new Stack<IngredientAnimator>(_freeSlots.Count);
        }

        private void Cleanup()
        {
            _filledSlots.Clear();
            _freeSlots.Clear();
            _ingredientsAnimators.Clear();
        }
    }
}