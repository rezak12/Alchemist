using System.Threading.Tasks;
using Code.Infrastructure.States.PotionMakingStates;
using Code.Logic.PotionMaking;
using Code.Logic.Potions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.PotionMakingUI
{
    public class AlchemyTableInteractionPanel : MonoBehaviour
    {
        [SerializeField] private Button _removeLastIngredientButton;
        [SerializeField] private Button _createPotionButton;

        private AlchemyTable _table;
        private PotionMakingLevelStateMachine _stateMachine;
        private UnityAction _createPotionAction;

        [Inject]
        private void Construct(PotionMakingLevelStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Initialize(AlchemyTable table)
        {
            _table = table;
            
            _table.FilledSlotsAmountChanged += UpdateRemoveButtonInteractableState;
            _table.FilledSlotsAmountChanged += UpdateCreatePotionButtonInteractableState;
            
            _createPotionAction = UniTask.UnityAction(CreatePotion);
            _removeLastIngredientButton.onClick.AddListener(RemoveLastIngredient);
            _createPotionButton.onClick.AddListener(_createPotionAction);

            UpdateRemoveButtonInteractableState();
            UpdateCreatePotionButtonInteractableState();
        }

        private void OnDestroy()
        {
            _table.FilledSlotsAmountChanged -= UpdateRemoveButtonInteractableState;
            _table.FilledSlotsAmountChanged -= UpdateCreatePotionButtonInteractableState;
            
            _removeLastIngredientButton.onClick.RemoveListener(RemoveLastIngredient);
            _createPotionButton.onClick.RemoveListener(_createPotionAction);
        }

        private async UniTaskVoid CreatePotion()
        {
            Potion potion = await _table.HandleResult();
            _stateMachine.Enter<OrderCompletedState, Potion>(potion).Forget();
        }

        private void RemoveLastIngredient()
        {
            _table.RemoveLastIngredient();
        }

        private void UpdateRemoveButtonInteractableState()
        {
            if (_table.IsAllSlotsFree)
            {
                _removeLastIngredientButton.interactable = false;
            }
            else if (!_removeLastIngredientButton.interactable)
            {
                _removeLastIngredientButton.interactable = true;
            }
        }

        private void UpdateCreatePotionButtonInteractableState()
        {
            if (_table.IsAllSlotsFree)
            {
                _createPotionButton.interactable = false;
            }
            else if (!_createPotionButton.interactable)
            {
                _createPotionButton.interactable = true;
            }
        }
    }
}