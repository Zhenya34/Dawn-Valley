using UnityEngine;

namespace UI.SampleScene.Inventory
{
    public class InventoryInitialization : MonoBehaviour
    {
        [SerializeField] private InventoryManager inventoryManager;

        private void Awake() => inventoryManager.InitializeInventory();
    }
}
