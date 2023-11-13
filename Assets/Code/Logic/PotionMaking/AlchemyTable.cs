using System;
using System.Collections.Generic;
using Code.Animations;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.VFX;
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
        private Stack<IngredientTweener> _ingredientAnimators;
        
        private IPotionInfoFactory _potionInfoFactory;
        private IPotionFactory _potionFactory;
        private IIngredientFactory _ingredientFactory;
        private IVFXProvider _vfxProvider;

        [Inject]
        private void Construct(
            IPotionInfoFactory potionInfoFactory,
            IIngredientFactory ingredientFactory,
            IPotionFactory potionFactory, 
            IVFXProvider vfxProvider)
        {
            _potionFactory = potionFactory;
            _potionInfoFactory = potionInfoFactory;
            _ingredientFactory = ingredientFactory;
            _vfxProvider = vfxProvider;
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
            RemoveLastIngredientPrefabFromSlot().Forget();
        }

        public async UniTask<Potion> HandleResult()
        {
            var ingredients = TakeAllIngredients();
            await MoveAllIngredientsToPotionCreatingPoint();
            
            Cleanup();
            
            Potion potion = await CreatePotion(ingredients);
            return potion;
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
            IngredientTweener ingredientTweener = await _ingredientFactory.CreateIngredientAsync(
                ingredientData.PrefabReference, _ingredientsSpawnPoint.position);
            
            _ingredientAnimators.Push(ingredientTweener);
            await ingredientTweener.JumpTo(slotTransform);

            VFX vfx = await _vfxProvider.Get(VFXType.Ingredient, slotTransform.position);
            vfx.Play().Forget();
        }

        private async UniTask MoveAllIngredientsToPotionCreatingPoint()
        {
            var tasks = new List<UniTask>(_ingredientAnimators.Count);
            foreach (IngredientTweener ingredientAnimator in _ingredientAnimators)
            {
                tasks.Add(ingredientAnimator.JumpTo(_potionSpawnPoint));
            }
            await UniTask.WhenAll(tasks);
            
            foreach (IngredientTweener ingredientAnimator in _ingredientAnimators)
            {
                Destroy(ingredientAnimator.gameObject);
            }
        }

        private async UniTaskVoid RemoveLastIngredientPrefabFromSlot()
        {
            IngredientTweener ingredientTweener = _ingredientAnimators.Pop();
            await ingredientTweener.JumpTo(_ingredientsRemoveFromSlotPoint);
            Destroy(ingredientTweener.gameObject);
        }

        private void InitializeSlotsCollections()
        {
            _freeSlots = new Stack<AlchemyTableSlot>(_tableSlots);
            _filledSlots = new Stack<AlchemyTableSlot>(_freeSlots.Count);
            _ingredientAnimators = new Stack<IngredientTweener>(_freeSlots.Count);
        }

        private void Cleanup()
        {
            _filledSlots.Clear();
            _freeSlots.Clear();
            _ingredientAnimators.Clear();
        }
    }
}