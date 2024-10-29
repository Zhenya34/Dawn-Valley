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

    private readonly Dictionary<Vector3Int, bool> _occupiedTiles = new();

    private readonly Dictionary<Vector3Int, PlantsGrowth> _plantsByTile = new();

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

    public void RegisterPlant(Vector3Int cellPosition, PlantsGrowth plant)
    {
        if (!_plantsByTile.ContainsKey(cellPosition))
        {
            _plantsByTile[cellPosition] = plant;
        }
    }

    public void UnregisterPlant(Vector3Int cellPosition)
    {
        if (_plantsByTile.ContainsKey(cellPosition))
        {
            _plantsByTile.Remove(cellPosition);
        }
    }

    public bool IsPlantAtTile(Vector3Int cellPosition)
    {
        return _plantsByTile.ContainsKey(cellPosition);
    }

    public PlantsGrowth GetPlantAtTile(Vector3Int cellPosition)
    {
        _plantsByTile.TryGetValue(cellPosition, out var plant);
        return plant;
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