using System.Collections.Generic;
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
            if (!uiManager.IsUIActive())
            {
                float distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance <= shopRadius)
                {
                    shopPanel.SetActive(true);
                    uiManager.ActivateUI();
                    sampleSceneCanvasLogic.SwitchOffPauseButton();
                }
            }
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
            if (_cart.ContainsKey(shopItem.itemName))
            {
                _cart[shopItem.itemName]++;
            }
            else
            {
                _cart[shopItem.itemName] = 1;
            }
            UpdateQuantityText(shopItem.itemName);
        }

        private void UpdateQuantityText(string itemName)
        {
            foreach (var shopItem in shopItems)
            {
                if (shopItem.itemName == itemName)
                {
                    shopItem.itemQuantityText.text = _cart[itemName].ToString();
                    break;
                }
            }
        }

        private void ConfirmPurchase()
        {
            int totalCost = 0;
            foreach (var entry in _cart)
            {
                Item item = itemDatabase.GetItemByName(entry.Key);
                if (item != null)
                {
                    totalCost += item.purchasePrice * entry.Value;
                }
            }

            if (playerCoinsWallet.SpendCoins(totalCost))
            {
                foreach (var entry in _cart)
                {
                    Item purchasedItem = itemDatabase.GetItemByName(entry.Key);
                    if (purchasedItem != null)
                    {
                        inventoryManager.AddItem(entry.Key, entry.Value);
                    }
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