using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private List<Item> _items = new();

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
}