using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WateringCanLogic : MonoBehaviour
{
    [SerializeField] private ToolSwitcher _toolSwitcher;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _dryDirtTile;
    [SerializeField] private TileBase _wetDirtTile;
    [SerializeField] private TileBase _dryGardenBedTile;
    [SerializeField] private TileBase _wetGardenBedTile;
    [SerializeField] private int _wateringCanUses = 5;
    [SerializeField] private int _wetDuration = 3;
    [SerializeField] private DayNightCycle _dayNightCycle;

    private readonly Dictionary<Vector3Int, int> _wateredPlots = new();

    private void Update()
    {
        HandleWatering();
    }

    private void HandleWatering()
    {
        if (Input.GetMouseButtonDown(1) && _toolSwitcher.GetCurrentTool() == ToolSwitcher.ToolType.WateringCan)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            Vector3Int gridPosition = _tilemap.WorldToCell(mouseWorldPos);
            TileBase clickedTile = _tilemap.GetTile(gridPosition);

            if (_wateringCanUses > 0)
            {
                if (clickedTile == _dryDirtTile)
                {
                    _tilemap.SetTile(gridPosition, _wetDirtTile);
                    UpdateWateringCan(gridPosition);
                }
                else if (clickedTile == _dryGardenBedTile)
                {
                    _tilemap.SetTile(gridPosition, _wetGardenBedTile);
                    UpdateWateringCan(gridPosition);
                }
            }
        }
    }

    private void UpdateWateringCan(Vector3Int gridPosition)
    {
        _wateringCanUses--;
        _wateredPlots[gridPosition] = _dayNightCycle.GetCurrentDay();
    }

    public void CheckForDryingPlots()
    {
        List<Vector3Int> plotsToDry = new();

        foreach (var plot in _wateredPlots)
        {
            if (_dayNightCycle.GetCurrentDay() >= plot.Value + _wetDuration)
            {
                plotsToDry.Add(plot.Key);
            }
        }

        foreach (var plot in plotsToDry)
        {
            TileBase currentTile = _tilemap.GetTile(plot);

            if (currentTile == _wetGardenBedTile)
            {
                _tilemap.SetTile(plot, _dryGardenBedTile);
            }
            else if (currentTile == _wetDirtTile)
            {
                _tilemap.SetTile(plot, _dryDirtTile);
            }

            _wateredPlots.Remove(plot);
        }
    }

    public void RefillWateringCan()
    {
        _wateringCanUses = 5;
    }
}