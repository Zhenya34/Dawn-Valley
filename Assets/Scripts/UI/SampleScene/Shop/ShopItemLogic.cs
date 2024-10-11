using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopItem
{
    public string itemName;
    public Button itemButton;
    public TextMeshProUGUI itemQuantityText;
}

public class ShopItemLogic : MonoBehaviour
{
    [SerializeField] private ShopItem[] _shopItems;
    [SerializeField] private PlayerCoinsWallet _playerCoinsWallet;
    [SerializeField] private SampleSceneCanvasLogic _sampleSceneCanvasLogic;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField] private float _shopRadius;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private UIManager _uiManager;

    private readonly Dictionary<string, int> _cart = new();

    private void Start()
    {
        InitializeShop();
    }

    private void OnMouseDown()
    {
        if (!_uiManager.IsUIActive())
        {
            float distance = Vector3.Distance(_player.transform.position, transform.position);
            if (distance <= _shopRadius)
            {
                _shopPanel.SetActive(true);
                _uiManager.ActivateUI();
                _sampleSceneCanvasLogic.SwitchOffPauseButton();
            }
        }
    }

    private void InitializeShop()
    {
        foreach (var shopItem in _shopItems)
        {
            shopItem.itemButton.onClick.AddListener(() => AddToCart(shopItem));
            shopItem.itemQuantityText.text = "0";

            _confirmButton.onClick.AddListener(ConfirmPurchase);
            _cancelButton.onClick.AddListener(CancelPurchase);
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
        foreach (var shopItem in _shopItems)
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
            Item item = _itemDatabase.GetItemByName(entry.Key);
            if (item != null)
            {
                totalCost += item.purchasePrice * entry.Value;
            }
        }

        if (_playerCoinsWallet.SpendCoins(totalCost))
        {
            foreach (var entry in _cart)
            {
                Item purchasedItem = _itemDatabase.GetItemByName(entry.Key);
                if (purchasedItem != null)
                {
                    _inventoryManager.AddItem(entry.Key, entry.Value);
                }
            }
            ClearCart();
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    private void CancelPurchase()
    {
        ClearCart();
    }

    private void ClearCart()
    {
        _cart.Clear();
        foreach (var shopItem in _shopItems)
        {
            shopItem.itemQuantityText.text = "0";
        }
    }
}