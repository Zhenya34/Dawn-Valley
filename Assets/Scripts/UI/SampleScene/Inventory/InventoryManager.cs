using System.Collections.Generic;
using Animals.Pets.PetsActivator;
using UI.SampleScene.Shop;
using UnityEngine;

namespace UI.SampleScene.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private SellingItemsLogic sellingItemsLogic;
        [SerializeField] private ItemUsageManager itemUsageLogic;
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private ItemDatabase itemDatabase;
        [SerializeField] private SellingSlot sellingSlot;
        [SerializeField] private PetInventorySlot petInventorySlot;
        [SerializeField] private AllPetsActivator allPetsActivator;

        private readonly List<InventorySlot> _slots = new();
        private InventorySlot _selectedSlot;
        private const int MaxStackSize = 60;
        private bool _isItemSelected;

        public void InitializeInventory()
        {
            if (inventoryUI)
            {
                foreach (Transform slotTransform in inventoryUI.transform)
                {
                    if (slotTransform.TryGetComponent<InventorySlot>(out var slot))
                    {
                        _slots.Add(slot);
                    }
                }
            }
        }

        public void AddItem(string itemName, int quantity)
        {
            Item item = itemDatabase.GetItemByName(itemName);
            if (item != null)
            {
                foreach (InventorySlot slot in _slots)
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

                foreach (InventorySlot slot in _slots)
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

            if (_selectedSlot)
            {
                _selectedSlot.Deselect();
            }

            _selectedSlot = slot;
            _selectedSlot.Select();
            _isItemSelected = false;
        }

        public void OnSlotLeftClicked(InventorySlot slot)
        {
            if (_selectedSlot && _isItemSelected)
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

        public void OnSlotRightClicked(InventorySlot slot)
        {
            if (_selectedSlot == slot && _isItemSelected)
            {
                itemUsageLogic.UseSelectedItem(slot);
            }
        }

        public void OnSellingSlotClicked(SellingSlot slot)
        {
            if (_selectedSlot && _isItemSelected)
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
            if (_selectedSlot && _isItemSelected)
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
            Item selectedItem = itemDatabase.GetItemBySprite(itemSprite);

            if (selectedItem != null)
            {
                sellingSlot.SetItem(itemSprite, itemQuantity);
                _selectedSlot.ClearSlot();
                DeselectSlot();
            }
        }

        public void MoveItemToInventory(SellingSlot slot)
        {
            if (!_selectedSlot && _isItemSelected && slot.GetItemSprite())
            {
                Sprite itemSprite = slot.GetItemSprite();
                int itemQuantity = slot.GetQuantity();
                Item selectedItem = itemDatabase.GetItemBySprite(itemSprite);
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
            Item selectedItem = itemDatabase.GetItemBySprite(itemSprite);

            if (selectedItem != null && selectedItem.itemType == Item.ItemType.Pet)
            {
                petInventorySlot.SetItem(itemSprite);
                _selectedSlot.ClearSlot();
                DeselectSlot();
                allPetsActivator.ActivatePet(selectedItem.itemName);
            }
        }

        public void MovePetItemToInventory(PetInventorySlot petSlot)
        {
            if (!petSlot.IsEmpty())
            {
                Sprite itemSprite = petSlot.GetItemSprite();
                Item selectedItem = itemDatabase.GetItemBySprite(itemSprite);

                if (selectedItem != null && selectedItem.itemType == Item.ItemType.Pet)
                {
                    AddItem(selectedItem.itemName, 1);
                    petSlot.ClearSlot();
                    DeselectSlot();
                    allPetsActivator.DeactivatePet(selectedItem.itemName);
                }
            }
        }

        public List<InventorySlot> GetAllSlots()
        {
            return _slots;
        }

        private void SelectSellingSlot(SellingSlot slot)
        {
            if (slot.GetItemSprite())
            {
                slot.Select();
                _isItemSelected = true;
            }
        }

        private void SelectPetSlot(PetInventorySlot slot)
        {
            if (slot.GetItemSprite())
            {
                slot.Select();
                _isItemSelected = true;
            }
        }

        private void DeselectSlot()
        {
            if (_selectedSlot)
            {
                _selectedSlot.Deselect();
            }

            _isItemSelected = false;
            _selectedSlot = null;
        }
    }
}