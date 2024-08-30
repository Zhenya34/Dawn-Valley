using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private SellingItemsLogic _sellingItemsLogic;
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField] private SellingSlot _sellingSlot;
    [SerializeField] private PetInventorySlot _petInventorySlot;
    [SerializeField] private AllPetsActivator _allPetsActivator;

    private readonly List<InventorySlot> _slots = new();
    private InventorySlot _selectedSlot;
    private const int _maxStackSize = 60;
    private bool _isItemSelected;

    private void Awake()
    {
        if (_inventoryUI == null || _itemDatabase == null)
        {
            return;
        }

        foreach (Transform slotTransform in _inventoryUI.transform)
        {
            if (slotTransform.TryGetComponent<InventorySlot>(out var slot))
            {
                _slots.Add(slot);
            }
        }
    }

    public void AddItem(string itemName, int quantity)
    {
        Item item = _itemDatabase.GetItemByName(itemName);
        if (item != null)
        {
            foreach (InventorySlot slot in _slots)
            {
                if (slot.GetItemSprite() == item.itemSprite)
                {
                    int newQuantity = slot.GetQuantity() + quantity;
                    if (newQuantity <= _maxStackSize)
                    {
                        slot.UpdateQuantity(newQuantity);
                        return;
                    }
                    else
                    {
                        int remaining = newQuantity - _maxStackSize;
                        slot.UpdateQuantity(_maxStackSize);
                        quantity = remaining;
                    }
                }
            }

            foreach (InventorySlot slot in _slots)
            {
                if (slot.IsEmpty())
                {
                    if (quantity > _maxStackSize)
                    {
                        slot.SetItem(item.itemSprite, _maxStackSize);
                        quantity -= _maxStackSize;
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
        Item item = _itemDatabase.GetItemByName(itemName);
        if (item != null)
        {
            foreach (InventorySlot slot in _slots)
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

    private void SelectSlot(InventorySlot slot)
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
        if (_selectedSlot != null && _isItemSelected)
        {
            if (slot.IsEmpty())
            {
                slot.SetItem(_selectedSlot.GetItemSprite(), _selectedSlot.GetQuantity());
                _selectedSlot.ClearSlot();
                DeselectSlot();
            }
            else
            {
                SelectSlot(slot);
            }
        }
        else
        {
            if (!slot.IsEmpty())
            {
                SelectSlot(slot);
            }
        }
    }

    public void OnSellingSlotClicked(SellingSlot slot)
    {
        if (_selectedSlot != null && _isItemSelected)
        {
            if (slot.IsEmpty())
            {
                MoveItemToSellingSlot();
            }
        }
        else
        {
            if (!slot.IsEmpty())
            {
                SelectSellingSlot(slot);
            }
        }
    }

    public void OnPetSlotClicked(PetInventorySlot slot)
    {
        if (_selectedSlot != null && _isItemSelected)
        {
            if (slot.IsEmpty())
            {
                MoveItemToPetSlot();
            }
        }
        else
        {
            if (!slot.IsEmpty())
            {
                SelectPetSlot(slot);
            }
        }
    }

    private void MoveItemToSellingSlot()
    {
        Sprite itemSprite = _selectedSlot.GetItemSprite();
        int itemQuantity = _selectedSlot.GetQuantity();
        Item selectedItem = _itemDatabase.GetItemBySprite(itemSprite);

        if (selectedItem != null)
        {
            _sellingSlot.SetItem(itemSprite, itemQuantity);
            _selectedSlot.ClearSlot();
            DeselectSlot();
        }
    }

    public void MoveItemToInventory(SellingSlot slot)
    {
        if (_selectedSlot == null && _isItemSelected && slot.GetItemSprite() != null)
        {
            Sprite itemSprite = slot.GetItemSprite();
            int itemQuantity = slot.GetQuantity();
            Item selectedItem = _itemDatabase.GetItemBySprite(itemSprite);
            if (selectedItem != null)
            {
                AddItem(selectedItem.itemName, itemQuantity);
                slot.ClearSlot();
                DeselectSlot();
            }
        }
        else if (!slot.IsEmpty())
        {
            SelectSellingSlot(slot);
        }
    }

    private void MoveItemToPetSlot()
    {
        Sprite itemSprite = _selectedSlot.GetItemSprite();
        Item selectedItem = _itemDatabase.GetItemBySprite(itemSprite);

        if (selectedItem != null && selectedItem.itemType == Item.ItemType.Pet)
        {
            _petInventorySlot.SetItem(itemSprite);
            _selectedSlot.ClearSlot();
            DeselectSlot();
            _allPetsActivator.ActivatePet(selectedItem.itemName);
        }
    }

    public void MovePetItemToInventory(PetInventorySlot petSlot)
    {
        if (!petSlot.IsEmpty())
        {
            Sprite itemSprite = petSlot.GetItemSprite();
            Item selectedItem = _itemDatabase.GetItemBySprite(itemSprite);

            if (selectedItem != null && selectedItem.itemType == Item.ItemType.Pet)
            {
                AddItem(selectedItem.itemName, 1);
                petSlot.ClearSlot();
                DeselectSlot();
                _allPetsActivator.DeactivatePet(selectedItem.itemName);
            }
        }
    }

    private void SelectSellingSlot(SellingSlot slot)
    {
        if (slot.GetItemSprite() != null)
        {
            slot.Select();
            _isItemSelected = true;
        }
    }

    private void SelectPetSlot(PetInventorySlot slot)
    {
        if (slot.GetItemSprite() != null)
        {
            slot.Select();
            _isItemSelected = true;
        }
    }

    private void DeselectSlot()
    {
        if (_selectedSlot != null)
        {
            _selectedSlot.Deselect();
        }

        _isItemSelected = false;
        _selectedSlot = null;
    }
}