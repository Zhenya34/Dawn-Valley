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
        if (Input.GetKeyDown(KeyCode.Y))
        {
            _inventoryManager.AddItem("SlimePet1", 1);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            _inventoryManager.AddItem("SlimePet2", 1);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            _inventoryManager.AddItem("StonePet", 1);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            _inventoryManager.AddItem("GhostPet", 1);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            _inventoryManager.AddItem("BeePet", 1);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            _inventoryManager.AddItem("CatPet1", 1);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            _inventoryManager.AddItem("CatPet2", 1);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            _inventoryManager.AddItem("AxolotlPet", 1);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            _inventoryManager.AddItem("WhiteFoxPet", 1);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            _inventoryManager.AddItem("PumpkinPet", 1);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            _inventoryManager.AddItem("FrogPet", 1);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            _inventoryManager.AddItem("ChickPet", 1);
        }
        
    }
}
