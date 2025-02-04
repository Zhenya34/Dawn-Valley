using System.Collections.Generic;
using System.Linq;
using Enviroment.Plants;
using UnityEngine;

namespace UI.SampleScene.Inventory
{
    public class ItemDatabase : MonoBehaviour
    {
        [SerializeField] private List<Item> items = new();
        [SerializeField] private Seed[] seeds;

        public Seed GetSeedByItem(Item item)
        {
            return seeds.FirstOrDefault(seed => seed.seedName == item.itemName);
        }

        public Item GetItemByName(string name)
        {
            return items.Find(item => item.itemName == name);
        }

        public Item GetItemBySprite(Sprite sprite)
        {
            return items.Find(item => item.itemSprite == sprite);
        }
    }

    [System.Serializable]
    public class Item
    {
        public string itemName;
        public Sprite itemSprite;
        public int purchasePrice;
        public int sellingPrice;
        public ItemType itemType;
        public GlobalItemType globalItemType;

        public enum GlobalItemType
        {
            None,
            Seed,
            Fence,
            Wicket,
            Structure
        }

        public enum ItemType
        {
            General,
            Pet
        }
    }
}