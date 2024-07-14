using System.Collections.Generic;
using UnityEngine;

public class SellingItemsLogic : MonoBehaviour
{
    public GameObject sellingUI;
    public ItemDatabase itemDatabase;
    public InventoryManager inventoryManager;
    public PlayerCoinsWallet playerWallet;

    private readonly List<SellingSlot> sellingSlots = new();

    private void Awake()
    {
        if (sellingUI == null || itemDatabase == null || inventoryManager == null || playerWallet == null)
        {
            return;
        }

        foreach (Transform slotTransform in sellingUI.transform)
        {
            if (slotTransform.TryGetComponent<SellingSlot>(out var slot))
            {
                sellingSlots.Add(slot);
            }
        }
    }

    public void AddItemToSell(string itemName, int quantity)
    {
        Item item = itemDatabase.GetItemByName(itemName);
        if (item != null)
        {
            foreach (SellingSlot slot in sellingSlots)
            {
                if (slot.IsEmpty())
                {
                    slot.SetItem(item.itemSprite, quantity);
                    inventoryManager.RemoveItem(itemName, quantity);
                    return;
                }
            }
        }
    }

    public void SellItems()
    {
        foreach (SellingSlot slot in sellingSlots)
        {
            if (!slot.IsEmpty())
            {
                string itemName = itemDatabase.items.Find(i => i.itemSprite == slot.GetItemSprite()).itemName;
                int itemPrice = GetItemPrice(itemName);
                int quantity = slot.GetQuantity();
                int totalPrice = itemPrice * quantity;

                playerWallet.AddCoins(totalPrice);
                slot.ClearSlot();
            }
        }
    }

    private int GetItemPrice(string itemName)
    {
        // Implement your logic to fetch the item price from the database
        // Here I'm returning a placeholder value
        return 10; // Placeholder price
    }
}