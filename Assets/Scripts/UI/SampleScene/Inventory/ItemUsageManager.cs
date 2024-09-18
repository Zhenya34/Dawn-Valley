using UnityEngine;

public class ItemUsageManager : MonoBehaviour
{
    [SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField] private Planting _plantingSystem;

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
        DefineTheObject(item, slot);
    }

    private void DefineTheObject(Item item, InventorySlot slot)
    {
        if (item.globalItemType == Item.GlobalItemType.Seed)
        {
            UseSeed(item, slot);
        }
        else if(item.globalItemType == Item.GlobalItemType.Structure)
        {

        }
        else if(item.globalItemType == Item.GlobalItemType.None)
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
    }

    private void UseSeed(Item item, InventorySlot slot)
    {
        int itemQuantity = slot.GetQuantity();

        if (itemQuantity <= 0)
        {
            StopUsingItem();
            return;
        }

        Seed seed = _itemDatabase.GetSeedByItem(item);

        if (seed != null)
        {
            _plantingSystem.PlantSeed(seed.plant, slot);
        }
    }

    public void UpdateCountOfSeeds(InventorySlot slot)
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
}
