using Code.Logic.PotionMaking;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.PotionMakingUI
{
    public class AlchemyTableInteractionPanel : MonoBehaviour
    {
        [SerializeField] private Button _removeLastIngredientButton;
        [SerializeField] private Button _createPotionButton;

        private AlchemyTable _table;

        public void Initialize(AlchemyTable table)
        {
            _table = table;
            
            _table.FilledSlotsAmountChanged += UpdateRemoveButtonInteractableState;
            _table.FilledSlotsAmountChanged += UpdateCreatePotionButtonInteractableState;
            
            _removeLastIngredientButton.onClick.AddListener(RemoveLastIngredient);
            _createPotionButton.onClick.AddListener(CreatePotion);

            UpdateRemoveButtonInteractableState();
            UpdateCreatePotionButtonInteractableState();
        }

        private void OnDestroy()
        {
            _table.FilledSlotsAmountChanged -= UpdateRemoveButtonInteractableState;
            _table.FilledSlotsAmountChanged -= UpdateCreatePotionButtonInteractableState;
            
            _removeLastIngredientButton.onClick.RemoveListener(RemoveLastIngredient);
            _createPotionButton.onClick.RemoveListener(CreatePotion);
        }

        private void CreatePotion()
        {
            _table.HandleResult().Forget();
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