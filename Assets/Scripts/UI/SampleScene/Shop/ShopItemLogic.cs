using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI.SampleScene.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SampleScene.Shop
{
    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public Button itemButton;
        public TextMeshProUGUI itemQuantityText;
    }

    public class ShopItemLogic : MonoBehaviour
    {
        [SerializeField] private ShopItem[] shopItems;
        [SerializeField] private PlayerCoinsWallet playerCoinsWallet;
        [SerializeField] private SampleSceneCanvasLogic sampleSceneCanvasLogic;
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private ItemDatabase itemDatabase;
        [SerializeField] private float shopRadius;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private UIManager.UIManager uiManager;

        private readonly Dictionary<string, int> _cart = new();

        private void Start() => InitializeShop();

        private void OnMouseDown()
        {
            if (uiManager.IsUIActive()) return;
            var distance = Vector3.Distance(player.transform.position, transform.position);
            if (!(distance <= shopRadius)) return;
            shopPanel.SetActive(true);
            uiManager.ActivateUI();
            sampleSceneCanvasLogic.SwitchOffPauseButton();
        }

        private void InitializeShop()
        {
            foreach (var shopItem in shopItems)
            {
                shopItem.itemButton.onClick.AddListener(() => AddToCart(shopItem));
                shopItem.itemQuantityText.text = "0";

                confirmButton.onClick.AddListener(ConfirmPurchase);
                cancelButton.onClick.AddListener(CancelPurchase);
            }
        }

        private void AddToCart(ShopItem shopItem)
        {
            if (!_cart.TryAdd(shopItem.itemName, 1))
            {
                _cart[shopItem.itemName]++;
            }

            UpdateQuantityText(shopItem.itemName);
        }

        private void UpdateQuantityText(string itemName)
        {
            foreach (var shopItem in shopItems)
            {
                if (shopItem.itemName != itemName) continue;
                shopItem.itemQuantityText.text = _cart[itemName].ToString();
                break;
            }
        }

        private void ConfirmPurchase()
        {
            var totalCost = (from entry in _cart let item = itemDatabase.GetItemByName(entry.Key) where item != null select item.purchasePrice * entry.Value).Sum();

            if (playerCoinsWallet.SpendCoins(totalCost))
            {
                foreach (var entry in from entry in _cart let purchasedItem = itemDatabase.GetItemByName(entry.Key) where purchasedItem != null select entry)
                {
                    inventoryManager.AddItem(entry.Key, entry.Value);
                }

                ClearCart();
            }
            else
            {
                Debug.Log("Not enough coins!");
            }
        }

        private void CancelPurchase() => ClearCart();

        private void ClearCart()
        {
            _cart.Clear();
            foreach (var shopItem in shopItems)
            {
                shopItem.itemQuantityText.text = "0";
            }
        }
    }
}