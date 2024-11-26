using System.Collections.Generic;
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
            foreach (Seed seed in seeds)
            {
                if (seed.seedName == item.itemName)
                {
                    return seed;
                }
            }

            return null;
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