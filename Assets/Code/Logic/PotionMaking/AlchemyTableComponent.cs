using System;
using System.Collections.Generic;
using Code.Animations;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.Pool;
using Code.Infrastructure.Services.SFX;
using Code.Infrastructure.Services.VFX;
using Code.Logic.Potions;
using Code.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Logic.PotionMaking
{
    public class AlchemyTableComponent : MonoBehaviour
    {
        [SerializeField] private AlchemyTableSlot[] _tableSlots;
        [SerializeField] private Transform _ingredientsSpawnPoint;
        [SerializeField] private Transform _ingredientsRemoveFromSlotPoint;
        [SerializeField] private Transform _potionSpawnPoint;
        
        public event Action FilledSlotsAmountChanged;
        public bool IsAllSlotsFilled => _alchemyTable.IsAllSlotsFilled;
        public bool IsAllSlotsFree => _alchemyTable.IsAllSlotsFree;

        private AlchemyTable _alchemyTable;
        private Stack<IngredientTweener> _ingredientTweeners;
        
        private IPotionFactory _potionFactory;
        private IIngredientFactory _ingredientFactory;
        private IVFXProvider _vfxProvider;
        private ISFXProvider _sfxProvider;

        [Inject]
        private void Construct(
            IPotionInfoFactory potionInfoFactory,
            IIngredientFactory ingredientFactory,
            IPotionFactory potionFactory, 
            IVFXProvider vfxProvider,
            ISFXProvider sfxProvider)
        {
            _alchemyTable = new AlchemyTable(potionInfoFactory, _tableSlots);
            _potionFactory = potionFactory;
            _ingredientFactory = ingredientFactory;
            _vfxProvider = vfxProvider;
            _sfxProvider = sfxProvider;
        }

        public void Initialize()
        {
            _ingredientTweeners = new Stack<IngredientTweener>(_tableSlots.Length);
        }

        public void AddIngredient(IngredientData ingredient)
        {
            _alchemyTable.FillSlot(ingredient);
            FilledSlotsAmountChanged?.Invoke();

            Transform slotTransform = _alchemyTable.LastFilledSlot.transform;
            MoveNewIngredientToSlot(ingredient, slotTransform).Forget();
        }

        public void RemoveLastIngredient()
        {
            _alchemyTable.ReleaseLastSlot();
            FilledSlotsAmountChanged?.Invoke();
            
            RemoveLastIngredientPrefabFromSlot().Forget();
        }

        public async UniTask<Potion> HandleResult()
        {
            PotionInfo potionInfo = await _alchemyTable.CreatePotionInfo();
            FilledSlotsAmountChanged?.Invoke();
            
            await MoveAllIngredientsToPotionCreatingPoint();
            Cleanup();
            
            Potion potion = await CreatePotion(potionInfo);
            var tweener = potion.GetComponent<PotionTweener>();
            Vector3 potionPosition = potion.transform.position;
            
            await UniTask.WhenAll(
                tweener.PresentAfterCreating(),
                _vfxProvider.Play(PoolObjectType.PotionVFX, potionPosition),
                _sfxProvider.PlayOneShot(potion.SFXReference, potionPosition));

            return potion;
        }

        private async UniTask<Potion> CreatePotion(PotionInfo potionInfo)
        {
            return await _potionFactory.CreatePotionAsync(potionInfo, _potionSpawnPoint.position);
        }

        private async UniTaskVoid MoveNewIngredientToSlot(IngredientData ingredientData, Transform slotTransform)
        {
            IngredientTweener ingredientTweener = await _ingredientFactory.CreateIngredientAsync(
                ingredientData.PrefabReference, _ingredientsSpawnPoint.position);
            _ingredientTweeners.Push(ingredientTweener);
            
            await ingredientTweener.JumpTo(slotTransform);
            await UniTask.WhenAll(
                _vfxProvider.Play(PoolObjectType.IngredientVFX, slotTransform.position),
               _sfxProvider.PlayOneShot(ingredientData.AudioClip, ingredientTweener.transform.position));
        }

        private async UniTask MoveAllIngredientsToPotionCreatingPoint()
        {
            var tasks = new List<UniTask>(_ingredientTweeners.Count);
            foreach (IngredientTweener ingredientAnimator in _ingredientTweeners)
            {
                tasks.Add(ingredientAnimator.JumpTo(_potionSpawnPoint));
            }
            await UniTask.WhenAll(tasks);
            
            foreach (IngredientTweener ingredientAnimator in _ingredientTweeners)
            {
                Destroy(ingredientAnimator.gameObject);
            }
        }

        private async UniTaskVoid RemoveLastIngredientPrefabFromSlot()
        {
            IngredientTweener ingredientTweener = _ingredientTweeners.Pop();
            await ingredientTweener.JumpTo(_ingredientsRemoveFromSlotPoint);
            Destroy(ingredientTweener.gameObject);
        }

        private void Cleanup()
        {
            _ingredientTweeners.Clear();
        }
    }
}