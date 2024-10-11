using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField] private InventoryManager _inventoryManager;

    public void CollectItem(Sprite itemSprite)
    {
        Item item = _itemDatabase.GetItemBySprite(itemSprite);

        if (item != null)
        {
            _inventoryManager.AddItem(item.itemName, 1);
        }
    }
}
