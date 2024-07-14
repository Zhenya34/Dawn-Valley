using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new();

    public Item GetItemByName(string name)
    {
        return items.Find(item => item.itemName == name);
    }
}

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite itemSprite;
}