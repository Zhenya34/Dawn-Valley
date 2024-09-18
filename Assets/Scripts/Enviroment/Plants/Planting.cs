using UnityEngine;
using UnityEngine.Tilemaps;

public class Planting : MonoBehaviour
{
    [SerializeField] private Player_Animation _playerAnim;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private ItemUsageManager _itemUsageManager;

    private Plant _currentPlant;
    private InventorySlot _currentSlot;
    private bool _canPlant = true;

    private void Update()
    {
        if (_canPlant)
        {
            if (Input.GetMouseButtonDown(1) && _playerAnim.GetToolsUsingValue() == true && _currentPlant != null)
            {
                Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0;
                Vector3Int cellPosition = _tilemap.WorldToCell(mouseWorldPos);

                Vector3 tileCenter = _tilemap.GetCellCenterWorld(cellPosition);

                Instantiate(_currentPlant.plantPrefab, tileCenter, Quaternion.identity);
                _itemUsageManager.UpdateCountOfSeeds(_currentSlot);
            }
        }
    }

    public void PlantSeed(Plant plant, InventorySlot slot)
    {
        _currentPlant = plant;
        _currentSlot = slot;
    }

    public void AllowPlanting()
    {
        _canPlant = true;
    }

    public void ForbidPlanting()
    {
        _canPlant = false;
    }
}
