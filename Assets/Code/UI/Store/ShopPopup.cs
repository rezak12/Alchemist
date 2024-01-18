using Code.Infrastructure.Services.StaticData;
using Code.StaticData.Shop;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.Store
{
    public class ShopPopup : MonoBehaviour
    {
        [SerializeField] private Button _showPotionsButton;
        [SerializeField] private Button _showTablesButton;
        [SerializeField] private Button _showEnvironmentsButton;
        [SerializeField] private Button _closePopup;
        [SerializeField] private ShopItemsContainer _itemsContainer;

        private ShopItemsCatalog _catalog;

        [Inject]
        private void Construct(IStaticDataService staticDataService) => 
            _catalog = staticDataService.GetShopItemsCatalog();

        public async UniTask InitializeAsync()
        {
            await _itemsContainer.InitializeAsync();
            _showPotionsButton.onClick.AddListener(UniTask.UnityAction(async () => await ShowPotions()));
            _showTablesButton.onClick.AddListener(UniTask.UnityAction(async () => await ShowTables()));
            _showEnvironmentsButton.onClick.AddListener(UniTask.UnityAction(async () => await ShowEnvironments()));
            _closePopup.onClick.AddListener(ClosePopup);

            await ShowPotions();
        }
        
        private void OnDestroy()
        {
            _showPotionsButton.onClick.RemoveAllListeners();
            _showTablesButton.onClick.RemoveAllListeners();
            _showEnvironmentsButton.onClick.RemoveAllListeners();
        }

        private async UniTask ShowPotions()
        {
            _showPotionsButton.interactable = false;
            await _itemsContainer.SetItems(_catalog.PotionItems);
            _showPotionsButton.interactable = true;
        }

        private async UniTask ShowTables()
        {
            _showTablesButton.interactable = false;
            await _itemsContainer.SetItems(_catalog.TableItems);
            _showTablesButton.interactable = true;
        }

        private async UniTask ShowEnvironments()
        {
            _showEnvironmentsButton.interactable = false;
            await _itemsContainer.SetItems(_catalog.EnvironmentItems);
            _showEnvironmentsButton.interactable = true;
        }

        private void ClosePopup() => Destroy(gameObject);
    }
}