using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private List<Item> _items = new();
    [SerializeField] private Seed[] _seeds;

    public Seed GetSeedByItem(Item item)
    {
        foreach (Seed seed in _seeds)
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
        return _items.Find(item => item.itemName == name);
    }

    public Item GetItemBySprite(Sprite sprite)
    {
        return _items.Find(item => item.itemSprite == sprite);
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
        Structure
    }

    public enum ItemType
    {
        General,
        Pet
    }
}