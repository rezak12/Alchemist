using System.Collections.Generic;
using Code.Infrastructure.Services.Factories;
using Code.Logic.Potions;
using Code.StaticData;
using Code.StaticData.Ingredients;
using Cysharp.Threading.Tasks;

namespace Code.Logic.PotionMaking
{
    public class AlchemyTable
    {
        public AlchemyTableSlot LastFilledSlot => _filledSlots.Peek();
        public bool IsAllSlotsFilled => _freeSlots.Count < 1;
        public bool IsAllSlotsFree => _filledSlots.Count < 1;
        
        private readonly Stack<AlchemyTableSlot> _freeSlots;
        private readonly Stack<AlchemyTableSlot> _filledSlots;
        private readonly IPotionInfoFactory _potionInfoFactory;

        public AlchemyTable(IPotionInfoFactory potionInfoFactory, IReadOnlyCollection<AlchemyTableSlot> slots)
        {
            _potionInfoFactory = potionInfoFactory;
            _freeSlots = new Stack<AlchemyTableSlot>(slots);
            _filledSlots = new Stack<AlchemyTableSlot>(slots.Count);
        }
        
        public void FillSlot(IngredientData ingredient)
        {
            AlchemyTableSlot slot = _freeSlots.Pop();
            
            slot.Fill(ingredient);
            _filledSlots.Push(slot);
        }
        
        public void ReleaseLastSlot()
        {
            AlchemyTableSlot slot = _filledSlots.Pop();
            
            slot.Release();
            _freeSlots.Push(slot);
        }
        
        public async UniTask<PotionInfo> CreatePotionInfo()
        {
            return await _potionInfoFactory.CreatePotionInfoAsync(GetAllIngredients());
        }
        
        private IEnumerable<IngredientData> GetAllIngredients()
        {
            var ingredients = new List<IngredientData>(_filledSlots.Count);
            foreach (AlchemyTableSlot slot in _filledSlots)
            {
                ingredients.Add(slot.CurrentIngredient);
                slot.Release();
            }
            
            Cleanup();

            return ingredients;
        }
        
        private void Cleanup()
        {
            _filledSlots.Clear();
            _freeSlots.Clear();
        }
    }
}