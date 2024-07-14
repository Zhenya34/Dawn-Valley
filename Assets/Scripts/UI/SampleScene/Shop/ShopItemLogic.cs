using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemLogic : MonoBehaviour
{
    public PlayerCoinsWallet playerCoinsWallet;
    public InventoryManager inventoryManager;
    public ItemDatabase itemDatabase;
    public Transform shopMenu;
    public GameObject shopButtonPrefab;

    private List<Item> cart = new List<Item>();
    private Dictionary<Item, int> cartQuantities = new Dictionary<Item, int>();

    private void Start()
    {
        PopulateShop();
    }

    private void PopulateShop()
    {
        System.Collections.IList list = itemDatabase.items;
        for (int i = 0; i < list.Count; i++)
        {
            Item item = (Item)list[i];
            GameObject button = Instantiate(shopButtonPrefab, shopMenu);
            button.transform.Find("ItemIcon").GetComponent<Image>().sprite = item.itemSprite;
            button.transform.Find("ItemPrice").GetComponent<Text>().text = item.itemPrice.ToString();
            button.GetComponent<Button>().onClick.AddListener(() => AddToCart(item));
        }
    }

    public void AddToCart(Item item)
    {
        if (cart.Contains(item))
        {
            cartQuantities[item]++;
        }
        else
        {
            cart.Add(item);
            cartQuantities[item] = 1;
        }
    }

    public void ClearCart()
    {
        cart.Clear();
        cartQuantities.Clear();
    }

    public void BuyAll()
    {
        int totalCost = 0;

        foreach (Item item in cart)
        {
            totalCost += item.itemPrice * cartQuantities[item];
        }

        if (playerCoinsWallet.CanAfford(totalCost))
        {
            playerCoinsWallet.DeductCoins(totalCost);

            foreach (Item item in cart)
            {
                inventoryManager.AddItem(item.itemName, cartQuantities[item]);
            }

            ClearCart();
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    [System.Serializable]
    public class Item
    {
        public string itemName;
        public Sprite itemSprite;
        public int itemPrice; // Цена товара
    }
}
