using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private ItemDatabase itemDatabase;

    private readonly List<InventorySlot> slots = new();
    private const int MaxStackSize = 60;
    private InventorySlot _selectedSlot;
    private bool _isItemSelected;

    private void Awake()
    {
        if (inventoryUI == null || itemDatabase == null)
        {
            return;
        }

        foreach (Transform slotTransform in inventoryUI.transform)
        {
            if (slotTransform.TryGetComponent<InventorySlot>(out var slot))
            {
                slots.Add(slot);
            }
        }
    }

    public void AddItem(string itemName, int quantity)
    {
        Item item = itemDatabase.GetItemByName(itemName);
        if (item != null)
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.GetItemSprite() == item.itemSprite)
                {
                    int newQuantity = slot.GetQuantity() + quantity;
                    if (newQuantity <= MaxStackSize)
                    {
                        slot.UpdateQuantity(newQuantity);
                        return;
                    }
                    else
                    {
                        int remaining = newQuantity - MaxStackSize;
                        slot.UpdateQuantity(MaxStackSize);
                        quantity = remaining;
                    }
                }
            }

            foreach (InventorySlot slot in slots)
            {
                if (slot.IsEmpty())
                {
                    if (quantity > MaxStackSize)
                    {
                        slot.SetItem(item.itemSprite, MaxStackSize);
                        quantity -= MaxStackSize;
                    }
                    else
                    {
                        slot.SetItem(item.itemSprite, quantity);
                        return;
                    }
                }
            }
        }
    }

    public void RemoveItem(string itemName, int quantity)
    {
        Item item = itemDatabase.GetItemByName(itemName);
        if (item != null)
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.GetItemSprite() == item.itemSprite)
                {
                    int currentQuantity = slot.GetQuantity();
                    if (currentQuantity >= quantity)
                    {
                        slot.UpdateQuantity(currentQuantity - quantity);
                        if (slot.GetQuantity() == 0)
                        {
                            slot.ClearSlot();
                        }
                        return;
                    }
                    else
                    {
                        slot.ClearSlot();
                        quantity -= currentQuantity;
                    }
                }
            }
        }
    }

    public void SelectSlot(InventorySlot slot)
    {
        if (slot.IsEmpty())
        {
            return;
        }

        if (_selectedSlot == slot)
        {
            _isItemSelected = true;
            return;
        }

        if (_selectedSlot != null)
        {
            _selectedSlot.Deselect();
        }

        _selectedSlot = slot;
        _selectedSlot.Select();
        _isItemSelected = false;
    }

    public void OnSlotClicked(InventorySlot slot)
    {
        if (_selectedSlot != null && _isItemSelected && slot.IsEmpty())
        {
            slot.SetItem(_selectedSlot.GetItemSprite(), _selectedSlot.GetQuantity());
            _selectedSlot.ClearSlot();
            DeselectSlot();
        }
        else if (!slot.IsEmpty())
        {
            SelectSlot(slot);
        }
    }

    private void DeselectSlot()
    {
        if (_selectedSlot != null)
        {
            _selectedSlot.Deselect();
            _selectedSlot = null;
        }
        _isItemSelected = false;
    }
}