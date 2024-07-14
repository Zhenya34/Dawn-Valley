using System;
using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    [SerializeField] private InventoryManager _inventoryManager;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            _inventoryManager.AddItem("Apple", 5);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            _inventoryManager.RemoveItem("Apple", 3);
        }

    }
}
