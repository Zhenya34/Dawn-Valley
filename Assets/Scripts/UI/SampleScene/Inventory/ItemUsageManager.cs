using UnityEngine;
using static Item;

public class ItemUsageManager : MonoBehaviour
{
    [SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField] private Planting _plantingSystem;
    [SerializeField] private FencesManager _fencesManager;
    [SerializeField] private WicketManager _wicketManager;
    [SerializeField] private InventoryManager _inventoryManager;

    private bool _isItemBeingUsed;
    private InventorySlot _currentSlot;

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

        _plantingSystem.AllowPlanting();
        _fencesManager.AllowFencesPlacement();
        _wicketManager.AllowWicketsPlacement();

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
        Item item = _itemDatabase.GetItemBySprite(itemSprite);
        return item;
    }

    private void StopUsingItem()
    {
        _isItemBeingUsed = false;
        _currentSlot = null;
        _plantingSystem.ForbidPlanting();
        _fencesManager.ForbidFencesPlacement();
        _wicketManager.ForbidWicketsPlacement();
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
            Seed seed = _itemDatabase.GetSeedByItem(item);

            if (seed != null)
            {
                _plantingSystem.PlantSeed(seed.plant, slot);
            }
        }
        else if (itemType == GlobalItemType.Fence)
        {
            _fencesManager.SetFence(slot);
        }
        else if (itemType == GlobalItemType.Wicket)
        {
            _wicketManager.SetWicket(slot);
        }
        else if (itemType == GlobalItemType.Structure)
        {
            
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
        foreach (InventorySlot slot in _inventoryManager.GetAllSlots())
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