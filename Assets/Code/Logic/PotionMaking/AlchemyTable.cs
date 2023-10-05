using System;
using System.Collections.Generic;
using Code.Animations;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.States.PotionMakingStates;
using Code.Logic.Potions;
using Code.StaticData;
using Cysharp.Threading.Tasks;
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
        private PotionMakingLevelStateMachine _stateMachine;

        [Inject]
        private void Construct(
            IPotionInfoFactory potionInfoFactory, 
            IIngredientFactory ingredientFactory,
            IPotionFactory potionFactory,
            PotionMakingLevelStateMachine stateMachine)
        {
            _potionFactory = potionFactory;
            _potionInfoFactory = potionInfoFactory;
            _ingredientFactory = ingredientFactory;
            _stateMachine = stateMachine;
        }

        public void Initialize()
        {
            InitializeSlotsCollections();
        }

        public void AddIngredient(IngredientData ingredient)
        {
            FillSlot(ingredient);
            MoveNewIngredientToSlot(ingredient, _filledSlots.Peek().transform).Forget();
        }

        public void RemoveLastIngredient()
        {
            ReleaseLastSlot();
            RemoveLastIngredientPrefabFromSlot();
        }

        public async UniTaskVoid HandleResult()
        {
            var ingredients = TakeAllIngredients();
            MoveAllIngredientsToPotionCreatingPoint();
            Cleanup();
            
            Potion potion = await CreatePotion(ingredients);
            await _stateMachine.Enter<OrderCompletedState, Potion>(potion);
        }

        private async UniTask<Potion> CreatePotion(IEnumerable<IngredientData> ingredients)
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

        private async UniTaskVoid MoveNewIngredientToSlot(IngredientData ingredientData, Transform slotTransform)
        {
            IngredientAnimator ingredientAnimator = await _ingredientFactory.CreateIngredientAsync(
                ingredientData.PrefabReference, _ingredientsSpawnPoint.position);
            
            _ingredientsAnimators.Push(ingredientAnimator);
            
            ingredientAnimator.MoveToSlot(slotTransform).Forget();
        }

        private void MoveAllIngredientsToPotionCreatingPoint()
        {
            foreach (IngredientAnimator ingredientAnimator in _ingredientsAnimators)
            {
                ingredientAnimator.RemoveFromSlotThenDestroy(_potionSpawnPoint).Forget();
            }
        }

        private void RemoveLastIngredientPrefabFromSlot()
        {
            IngredientAnimator ingredientAnimator = _ingredientsAnimators.Pop();
            ingredientAnimator.RemoveFromSlotThenDestroy(_ingredientsRemoveFromSlotPoint).Forget();
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