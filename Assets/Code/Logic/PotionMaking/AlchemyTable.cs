using System;
using System.Collections.Generic;
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
        
        public event Action FilledSlotsCountChanged;
        public bool IsAllSlotsFilled => _freeSlots.Count < 1;
        
        private Stack<AlchemyTableSlot> _freeSlots;
        private Stack<AlchemyTableSlot> _filledSlots;
        
        private IPotionFactory _potionFactory;

        [Inject]
        private void Construct(IPotionFactory potionFactory)
        {
            _potionFactory = potionFactory;

            InitializeTableSlotsCollection();
        }

        public void AddIngredient(IngredientData ingredient)
        {
            FillSlot(ingredient);
        }

        public void RemoveLastIngredient()
        {
            ReleaseLastSlot();
        }

        public void HandleResult()
        {
            var ingredients = new List<IngredientData>(_filledSlots.Count);
            
            foreach (AlchemyTableSlot slot in _filledSlots)
            {
                ingredients.Add(slot.CurrentIngredient);
                slot.Release();
            }
            Cleanup();

            PotionInfo potionInfo = _potionFactory.CreatePotion(ingredients);
        }

        private void ReleaseLastSlot()
        {
            AlchemyTableSlot slot = _filledSlots.Pop();
            
            slot.Release();
            _freeSlots.Push(slot);
            
            FilledSlotsCountChanged?.Invoke();
        }

        private void InitializeTableSlotsCollection()
        {
            _freeSlots = new Stack<AlchemyTableSlot>(_freeSlots);
            _filledSlots = new Stack<AlchemyTableSlot>(_freeSlots.Count);
        }

        private void FillSlot(IngredientData ingredient)
        {
            AlchemyTableSlot slot = _freeSlots.Pop();
            
            slot.Fill(ingredient);
            _filledSlots.Push(slot);
            
            FilledSlotsCountChanged?.Invoke();
        }

        private void Cleanup()
        {
            _filledSlots.Clear();
            _freeSlots.Clear();
        }
    }
}