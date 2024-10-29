using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HoeLogic : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _grassTile;
    [SerializeField] private TileBase _dirtTile;
    [SerializeField] private TileBase _gardenBed;
    [SerializeField] private TileBase _wetGardenBed;
    [SerializeField] private TileBase _wetDirtTile;
    [SerializeField] private ToolSwitcher _toolswitcher;
    [SerializeField] private DayNightCycle _dayNightCycle;
    [SerializeField] private PlayerImpactRadius _playerImpactRadius;
    [SerializeField] private int _daysBeforeDirtBecomeGrass;

    private readonly Dictionary<Vector3Int, int> _tilledSoilTiles = new();
    private enum ClickType { None, LeftClick, RightClick }
    private ClickType _lastClick = ClickType.None;

    public void RegisterMouseClick()
    {
        if (_toolswitcher.GetCurrentTool() == ToolSwitcher.ToolType.Hoe)
        {
            if (Input.GetMouseButton(0))
            {
                _lastClick = ClickType.LeftClick;
            }
            else if (Input.GetMouseButton(1))
            {
                _lastClick = ClickType.RightClick;
            }
        }
    }

    private void Update()
    {
        RegisterMouseClick();
        CheckForGrassRegrowth();
    }

    public void HandleHoeLogic()
    {
        if (_toolswitcher.GetCurrentTool() == ToolSwitcher.ToolType.Hoe)
        {
            HandleTileClick(out Vector3Int gridPosition, out TileBase clickedTile);

            Vector3 playerPosition = transform.position;
            Vector3 tileWorldPosition = _tilemap.CellToWorld(gridPosition);

            float distanceToTile = Vector3.Distance(playerPosition, tileWorldPosition);

            if (distanceToTile > _playerImpactRadius.GetToolDistanceValue())
            {
                return;
            }

            if (_lastClick == ClickType.RightClick)
            {
                if (clickedTile == _grassTile || clickedTile == _dirtTile)
                {
                    _tilemap.SetTile(gridPosition, _gardenBed);
                    _tilledSoilTiles.Remove(gridPosition);
                }
                else if (clickedTile == _wetDirtTile)
                {
                    _tilemap.SetTile(gridPosition, _wetGardenBed);
                    _tilledSoilTiles.Remove(gridPosition);
                }
            }
            if (_lastClick == ClickType.LeftClick)
            {
                if (clickedTile == _gardenBed)
                {
                    _tilemap.SetTile(gridPosition, _dirtTile);
                    _tilledSoilTiles[gridPosition] = _dayNightCycle.GetCurrentDay();
                    RemovePlantIfExists(gridPosition);
                }
                else if (clickedTile == _wetGardenBed)
                {
                    _tilemap.SetTile(gridPosition, _wetDirtTile);
                    _tilledSoilTiles[gridPosition] = _dayNightCycle.GetCurrentDay();
                    RemovePlantIfExists(gridPosition);
                }
            }

            _lastClick = ClickType.None;
            CheckForGrassRegrowth();
        }
    }

    private void RemovePlantIfExists(Vector3Int gridPosition)
    {
        var plantingSystem = FindObjectOfType<Planting>();
        if (plantingSystem != null && plantingSystem.IsPlantAtTile(gridPosition))
        {
            var plant = plantingSystem.GetPlantAtTile(gridPosition);
            if (plant != null)
            {
                Destroy(plant.gameObject);
            }
        }
    }

    private void CheckForGrassRegrowth()
    {
        List<Vector3Int> toRemove = new();
        List<Vector3Int> grassReady = new();

        foreach (var tilledTile in _tilledSoilTiles)
        {
            Vector3Int gridPosition = tilledTile.Key;
            int dayTilled = tilledTile.Value;
            int daysPassed = _dayNightCycle.GetCurrentDay() - dayTilled;

            TileBase currentTile = _tilemap.GetTile(gridPosition);

            if (currentTile == _wetGardenBed && daysPassed >= 1)
            {
                _tilemap.SetTile(gridPosition, _dirtTile);
                _tilledSoilTiles[gridPosition] = _dayNightCycle.GetCurrentDay();
            }
            else if (currentTile == _dirtTile)
            {
                if (daysPassed >= _daysBeforeDirtBecomeGrass)
                {
                    grassReady.Add(gridPosition);
                }
            }
        }

        foreach (var gridPosition in grassReady)
        {
            _tilemap.SetTile(gridPosition, _grassTile);
            toRemove.Add(gridPosition);
        }

        foreach (var gridPosition in toRemove)
        {
            _tilledSoilTiles.Remove(gridPosition);
        }
    }

    private void HandleTileClick(out Vector3Int gridPosition, out TileBase clickedTile)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        gridPosition = _tilemap.WorldToCell(mouseWorldPos);
        clickedTile = _tilemap.GetTile(gridPosition);
    }
}