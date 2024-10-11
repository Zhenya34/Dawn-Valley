using UnityEngine;

public class InventoryInitialization : MonoBehaviour
{
    [SerializeField] private InventoryManager _inventoryManager;

    private void Awake()
    {
        _inventoryManager.InitializeInventory();
    }
}
