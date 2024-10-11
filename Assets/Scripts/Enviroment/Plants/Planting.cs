using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Planting : MonoBehaviour
{
    [SerializeField] private Player_Animation _playerAnim;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _tilledTile;
    [SerializeField] private ItemUsageManager _itemUsageManager;
    [SerializeField] private DayNightCycle _dayNightCycle;

    private Plant _currentPlant;
    private InventorySlot _currentSlot;
    private bool _canPlant = true;

    private Dictionary<Vector3Int, bool> _occupiedTiles = new();

    private void Update()
    {
        if (_canPlant)
        {
            if (Input.GetMouseButtonDown(1) && _playerAnim.GetToolsUsingValue() == true && _currentPlant != null)
            {
                Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0;
                Vector3Int cellPosition = _tilemap.WorldToCell(mouseWorldPos);

                if (_tilemap.GetTile(cellPosition) == _tilledTile &&
                    (!_occupiedTiles.ContainsKey(cellPosition) || !_occupiedTiles[cellPosition]))
                {
                    Vector3 tileCenter = _tilemap.GetCellCenterWorld(cellPosition);

                    Instantiate(_currentPlant.plantPrefab, tileCenter, Quaternion.identity);

                    _occupiedTiles[cellPosition] = true;
                    _itemUsageManager.UpdateCountOfItem(_currentSlot);
                }
            }
        }
    }

    public void FreeCell(Vector3Int cellPosition)
    {
        if (_occupiedTiles.ContainsKey(cellPosition))
        {
            _occupiedTiles[cellPosition] = false;
        }
    }

    public Vector3Int GetCellPosition(Vector3 worldPosition)
    {
        return _tilemap.WorldToCell(worldPosition);
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