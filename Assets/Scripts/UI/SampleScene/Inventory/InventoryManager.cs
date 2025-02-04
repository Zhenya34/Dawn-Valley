using System.Collections.Generic;
using System.Linq;
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
            if (!inventoryUI) return;
            foreach (Transform slotTransform in inventoryUI.transform)
            {
                if (slotTransform.TryGetComponent<InventorySlot>(out var slot))
                {
                    _slots.Add(slot);
                }
            }
        }

        public void AddItem(string itemName, int quantity)
        {
            var item = itemDatabase.GetItemByName(itemName);
            if (item == null) return;
            foreach (var slot in _slots)
            {
                if (slot.GetItemSprite() != item.itemSprite) continue;
                var newQuantity = slot.GetQuantity() + quantity;
                if (newQuantity <= MaxStackSize)
                {
                    slot.UpdateQuantity(newQuantity);
                    return;
                }
                else
                {
                    var remaining = newQuantity - MaxStackSize;
                    slot.UpdateQuantity(MaxStackSize);
                    quantity = remaining;
                }
            }

            foreach (var slot in _slots.Where(slot => slot.IsEmpty()))
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

        public void RemoveItem(string itemName, int quantity)
        {
            var item = itemDatabase.GetItemByName(itemName);
            if (item == null) return;
            foreach (var slot in _slots)
            {
                if (slot.GetItemSprite() != item.itemSprite) continue;
                var currentQuantity = slot.GetQuantity();
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
            var itemSprite = _selectedSlot.GetItemSprite();
            var itemQuantity = _selectedSlot.GetQuantity();
            var selectedItem = itemDatabase.GetItemBySprite(itemSprite);

            if (selectedItem == null) return;
            sellingSlot.SetItem(itemSprite, itemQuantity);
            _selectedSlot.ClearSlot();
            DeselectSlot();
        }

        public void MoveItemToInventory(SellingSlot slot)
        {
            if (!_selectedSlot && _isItemSelected && slot.GetItemSprite())
            {
                var itemSprite = slot.GetItemSprite();
                var itemQuantity = slot.GetQuantity();
                var selectedItem = itemDatabase.GetItemBySprite(itemSprite);
                if (selectedItem == null) return;
                AddItem(selectedItem.itemName, itemQuantity);
                slot.ClearSlot();
                DeselectSlot();
            }
            else if (!slot.IsEmpty())
            {
                SelectSellingSlot(slot);
            }
        }

        private void MoveItemToPetSlot()
        {
            var itemSprite = _selectedSlot.GetItemSprite();
            var selectedItem = itemDatabase.GetItemBySprite(itemSprite);

            if (selectedItem is not { itemType: Item.ItemType.Pet }) return;
            petInventorySlot.SetItem(itemSprite);
            _selectedSlot.ClearSlot();
            DeselectSlot();
            allPetsActivator.ActivatePet(selectedItem.itemName);
        }

        public void MovePetItemToInventory(PetInventorySlot petSlot)
        {
            if (petSlot.IsEmpty()) return;
            var itemSprite = petSlot.GetItemSprite();
            var selectedItem = itemDatabase.GetItemBySprite(itemSprite);

            if (selectedItem is not { itemType: Item.ItemType.Pet }) return;
            AddItem(selectedItem.itemName, 1);
            petSlot.ClearSlot();
            DeselectSlot();
            allPetsActivator.DeactivatePet(selectedItem.itemName);
        }

        public List<InventorySlot> GetAllSlots()
        {
            return _slots;
        }

        private void SelectSellingSlot(SellingSlot slot)
        {
            if (!slot.GetItemSprite()) return;
            slot.Select();
            _isItemSelected = true;
        }

        private void SelectPetSlot(PetInventorySlot slot)
        {
            if (!slot.GetItemSprite()) return;
            slot.Select();
            _isItemSelected = true;
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