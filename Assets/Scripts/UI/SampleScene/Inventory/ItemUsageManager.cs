using Enviroment.Fences;
using Enviroment.Plants;
using Enviroment.Wicket;
using Player.Placement;
using UnityEngine;
using static UI.SampleScene.Inventory.Item;

namespace UI.SampleScene.Inventory
{
    public class ItemUsageManager : MonoBehaviour
    {
        [SerializeField] private ItemDatabase itemDatabase;
        [SerializeField] private Planting plantingSystem;
        [SerializeField] private FencesManager fencesManager;
        [SerializeField] private WicketManager wicketManager;
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private PlacementSystem placementSystem;
        [SerializeField] private ObjectsDatabaseSo objectsDatabaseSo;

        private bool _isItemBeingUsed;
        private InventorySlot _currentSlot;
        private int _structureID;

        public void UseSelectedItem(InventorySlot slot)
        {
            if (slot.IsEmpty())
            {
                return;
            }

            if (_isItemBeingUsed && _currentSlot == slot)
            {
                StopUsingItem();
                return;
            }

            Item item = GetItemFromSlot(slot);
            _currentSlot = slot;
            _isItemBeingUsed = true;

            plantingSystem.AllowPlanting();
            fencesManager.AllowFencesPlacement();
            wicketManager.AllowWicketsPlacement();

            DefineTheObject(item, slot);
        }

        private void DefineTheObject(Item item, InventorySlot slot)
        {
            if (item.globalItemType != GlobalItemType.None)
            {
                GlobalItemType itemType = item.globalItemType;
                UseItem(slot, item, itemType);
            }
            else if (item.globalItemType == GlobalItemType.None)
            {
                return;
            }
        }

        private Item GetItemFromSlot(InventorySlot slot)
        {
            Sprite itemSprite = slot.GetItemSprite();
            Item item = itemDatabase.GetItemBySprite(itemSprite);
            return item;
        }

        private void StopUsingItem()
        {
            _isItemBeingUsed = false;
            _currentSlot = null;
            plantingSystem.ForbidPlanting();
            fencesManager.ForbidFencesPlacement();
            wicketManager.ForbidWicketsPlacement();
            placementSystem.StopPlacement();
        }

        private void UseItem(InventorySlot slot, Item item ,GlobalItemType itemType)
        {
            int itemQuantity = slot.GetQuantity();

            if (itemQuantity <= 0)
            {
                StopUsingItem();
                return;
            }

            if(itemType == GlobalItemType.Seed)
            {
                Seed seed = itemDatabase.GetSeedByItem(item);

                if (seed != null)
                {
                    plantingSystem.PlantSeed(seed.plant, slot);
                }
            }
            else if (itemType == GlobalItemType.Fence)
            {
                fencesManager.SetFence(slot);
            }
            else if (itemType == GlobalItemType.Wicket)
            {
                wicketManager.SetWicket(slot);
            }
            else if (itemType == GlobalItemType.Structure)
            {
                _structureID = objectsDatabaseSo.GetStructureIDByName(item.itemName);
                placementSystem.StartPlacement(_structureID);
            }
        }

        public void UpdateCountOfItem(InventorySlot slot)
        {
            int itemQuantity = slot.GetQuantity();
            itemQuantity--;
            if (itemQuantity <= 0)
            {
                StopUsingItem();
                slot.ClearSlot();
                return;
            }
            else
            {
                slot.UpdateQuantity(itemQuantity);
            }
        }

        public bool HasItemInInventory(GlobalItemType itemType)
        {
            foreach (InventorySlot slot in inventoryManager.GetAllSlots())
            {
                if (!slot.IsEmpty())
                {
                    Item item = GetItemFromSlot(slot);
                    if (item.globalItemType == itemType)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}