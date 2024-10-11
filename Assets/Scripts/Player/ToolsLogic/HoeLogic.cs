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
    [SerializeField] private int _daysBeforeDirtBecomeGrass;

    private Dictionary<Vector3Int, int> _tilledSoilTiles = new();

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && _toolswitcher.GetCurrentTool() == ToolSwitcher.ToolType.Hoe)
        {
            HandleTileClick(out Vector3Int gridPosition, out TileBase clickedTile);

            if (clickedTile == _grassTile || clickedTile == _dirtTile)
            {
                _tilemap.SetTile(gridPosition, _gardenBed);
                _tilledSoilTiles[gridPosition] = _dayNightCycle.GetCurrentDay();
            }
            else if (clickedTile == _wetDirtTile)
            {
                _tilemap.SetTile(gridPosition, _wetGardenBed);
                _tilledSoilTiles[gridPosition] = _dayNightCycle.GetCurrentDay();
            }
        }

        if (Input.GetMouseButtonDown(0) && _toolswitcher.GetCurrentTool() == ToolSwitcher.ToolType.Hoe)
        {
            HandleTileClick(out Vector3Int gridPosition, out TileBase clickedTile);

            if (clickedTile == _gardenBed)
            {
                _tilemap.SetTile(gridPosition, _dirtTile);
            }
            if (clickedTile == _wetGardenBed)
            {
                _tilemap.SetTile(gridPosition, _wetDirtTile);
            }
        }

        //CheckForGrassRegrowth();
    }

/*    private void CheckForGrassRegrowth()
    {
        List<Vector3Int> toRemove = new();

        foreach (var tilledTile in _tilledSoilTiles)
        {
            int dayTilled = tilledTile.Value;
            if (_dayNightCycle.GetCurrentDay() - dayTilled >= _daysBeforeDirtBecomeGrass)
            {
                _tilemap.SetTile(tilledTile.Key, _grassTile);
                toRemove.Add(tilledTile.Key);
            }
        }

        foreach (var gridPosition in toRemove)
        {
            _tilledSoilTiles.Remove(gridPosition);
        }
    }*/

    private void HandleTileClick(out Vector3Int gridPosition, out TileBase clickedTile)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        gridPosition = _tilemap.WorldToCell(mouseWorldPos);
        clickedTile = _tilemap.GetTile(gridPosition);
    }
}