using System.Collections.Generic;
using Code.Infrastructure.Services.Factories;
using Code.Infrastructure.Services.Pool;
using Code.Infrastructure.Services.ProgressServices;
using Code.Infrastructure.Services.StaticData;
using Code.StaticData.Configs;
using Code.StaticData.Potions;
using Code.StaticData.Shop;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using Zenject;

namespace Code.UI.Store
{
    public class ShopItemsContainer : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private Transform _poolParent;

        private Pool<ShopItemView> _pool;
        private List<ShopItemView> _activeItems;
        
        private IStaticDataService _staticDataService;
        private IPersistentProgressService _progressService;

        [Inject]
        private void Construct(
            CachePrefabFactory prefabFactory, 
            IStaticDataService staticDataService, 
            IPersistentProgressService progressService)
        {
            _pool = new Pool<ShopItemView>(prefabFactory);
            _staticDataService = staticDataService;
            _progressService = progressService;
        }

        public async UniTask InitializeAsync()
        {
            _activeItems = new List<ShopItemView>();
            PoolObjectConfig poolConfig = _staticDataService.GetPoolConfigByType(PoolObjectType.ShopViewItem);
            await _pool.InitializeAsync(poolConfig.AssetReference, poolConfig.StartCapacity, poolConfig.Type, _poolParent);
        }

        public async UniTask SetItems(IEnumerable<PotionShopItem> items)
        {
            Cleanup();
            
            foreach (PotionShopItem shopItem in items)
            {
                ShopItemView viewItem = await _pool.Get(_parent.position, _parent);
                
                SetItem(viewItem, shopItem.Name, shopItem.Icon, shopItem.RequiredCoins, shopItem.RequiredReputation,
                    () => BuyPotion(shopItem.PotionReference, shopItem.RequiredCoins), 
                    () => SelectPotion(shopItem.PotionReference));

                if (!_progressService.IsPlayerOwnPotion(shopItem.PotionReference))
                {
                    continue;
                }
                
                viewItem.Unlock();
                if (_progressService.ChosenPotionDataReference.AssetGUID == shopItem.PotionReference.AssetGUID)
                {
                    viewItem.Select();
                }
            }
        }

        public async UniTask SetItems(IEnumerable<TableShopItem> items)
        {
            Cleanup();
            
            foreach (TableShopItem shopItem in items)
            {
                ShopItemView viewItem = await _pool.Get(_parent.position, _parent);
                SetItem(viewItem, shopItem.Name, shopItem.Icon, shopItem.RequiredCoins, shopItem.RequiredReputation,
                    () => BuyTable(shopItem.TableReference, shopItem.RequiredCoins), 
                    () => SelectTable(shopItem.TableReference));

                if (!_progressService.IsPlayerOwnTable(shopItem.TableReference))
                {
                    continue;
                }
                
                viewItem.Unlock();
                if (_progressService.ChosenAlchemyTablePrefabReference.AssetGUID == shopItem.TableReference.AssetGUID)
                {
                    viewItem.Select();
                }
            }
        }

        public async UniTask SetItems(IEnumerable<EnvironmentShopItem> items)
        {
            Cleanup();
            
            foreach (EnvironmentShopItem shopItem in items)
            {
                ShopItemView viewItem = await _pool.Get(_parent.position, _parent);
                SetItem(viewItem, shopItem.Name, shopItem.Icon, shopItem.RequiredCoins, shopItem.RequiredReputation,
                    () => BuyEnvironment(shopItem.EnvironmentReference, shopItem.RequiredCoins), 
                    () => SelectEnvironment(shopItem.EnvironmentReference));

                if (!_progressService.IsPlayerOwnEnvironment(shopItem.EnvironmentReference))
                {
                    continue;
                }
                
                viewItem.Unlock();
                if (_progressService.ChosenEnvironmentPrefabReference.AssetGUID == shopItem.EnvironmentReference.AssetGUID)
                {
                    viewItem.Select();
                }
            }
        }

        private void SetItem(
            ShopItemView viewItem,
            string itemName, 
            Sprite itemIcon, 
            int requiredCoinsAmount, 
            int requiredReputationAmount, 
            UnityAction onBuyButtonClicked,
            UnityAction onSelectButtonClicked)
        {
            viewItem.gameObject.SetActive(false);
                
            viewItem.SetItem(itemName, itemIcon, requiredCoinsAmount, requiredReputationAmount, 
                onBuyButtonClicked, onSelectButtonClicked);
            viewItem.Unselect();
            viewItem.Lock();
            
            viewItem.gameObject.SetActive(true);
            _activeItems.Add(viewItem);
        }

        private void BuyPotion(AssetReferenceT<PotionData> reference, int price)
        {
            _progressService.AddNewPotion(reference);
            _progressService.RemoveCoins(price);
        }

        private void SelectPotion(AssetReferenceT<PotionData> reference)
        {
            UnselectAllItems();
            _progressService.SetChosenPotion(reference);
        }

        private void BuyTable(AssetReferenceGameObject reference, int price)
        {
            _progressService.AddNewTable(reference);
            _progressService.RemoveCoins(price);
        }

        private void SelectTable(AssetReferenceGameObject reference)
        {
            UnselectAllItems();
            _progressService.SetChosenTable(reference);
        }

        private void BuyEnvironment(AssetReferenceGameObject reference, int price)
        {
            _progressService.AddNewEnvironment(reference);
            _progressService.RemoveCoins(price);
        }

        private void SelectEnvironment(AssetReferenceGameObject reference)
        {
            UnselectAllItems();
            _progressService.SetChosenEnvironment(reference);
        }

        private void UnselectAllItems()
        {
            foreach (ShopItemView shopItemView in _activeItems)
            {
                shopItemView.Unselect();
            }
        }

        private void Cleanup()
        {
            foreach (ShopItemView item in _activeItems)
            {
                _pool.Return(item);
            }
            _activeItems.Clear();
        }
    }
}