using UI.SampleScene.Inventory;
using UnityEngine;

namespace Enviroment.ItemCollecting
{
    public class ItemHandler : MonoBehaviour
    {
        [SerializeField] private ItemDatabase itemDatabase;
        [SerializeField] private InventoryManager inventoryManager;

        public void CollectItem(Sprite itemSprite)
        {
            Item item = itemDatabase.GetItemBySprite(itemSprite);

            if (item != null)
            {
                inventoryManager.AddItem(item.itemName, 1);
            }
        }
    }
}
