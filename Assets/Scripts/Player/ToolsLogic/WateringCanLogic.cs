using System.Collections.Generic;
using Enviroment.Time;
using UI.SampleScene;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Player.ToolsLogic
{
    public class WateringCanLogic : MonoBehaviour
    {
        [SerializeField] private ToolSwitcher toolSwitcher;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase dryDirtTile;
        [SerializeField] private TileBase wetDirtTile;
        [SerializeField] private TileBase dryGardenBedTile;
        [SerializeField] private TileBase wetGardenBedTile;
        [SerializeField] private int wateringCanUses = 5;
        [SerializeField] private int wetDuration = 3;
        [SerializeField] private DayNightCycle dayNightCycle;

        private readonly Dictionary<Vector3Int, int> _wateredPlots = new();

        public void HandleWatering()
        {
            if (toolSwitcher.GetCurrentTool() == ToolSwitcher.ToolType.WateringCan)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0;
                Vector3Int gridPosition = tilemap.WorldToCell(mouseWorldPos);
                TileBase clickedTile = tilemap.GetTile(gridPosition);

                if (wateringCanUses > 0)
                {
                    if (clickedTile == dryDirtTile)
                    {
                        tilemap.SetTile(gridPosition, wetDirtTile);
                        UpdateWateringCan(gridPosition);
                    }
                    else if (clickedTile == dryGardenBedTile)
                    {
                        tilemap.SetTile(gridPosition, wetGardenBedTile);
                        UpdateWateringCan(gridPosition);
                    }
                }
            }
        }

        public bool IsGrowingOnWetTile(Vector3Int cellPosition)
        {
            TileBase currentTile = tilemap.GetTile(cellPosition);
            return currentTile == wetDirtTile || currentTile == wetGardenBedTile;
        }

        private void UpdateWateringCan(Vector3Int gridPosition)
        {
            wateringCanUses--;
            _wateredPlots[gridPosition] = dayNightCycle.GetCurrentDay();
        }

        public void CheckForDryingPlots()
        {
            List<Vector3Int> plotsToDry = new();

            foreach (var plot in _wateredPlots)
            {
                if (dayNightCycle.GetCurrentDay() >= plot.Value + wetDuration)
                {
                    plotsToDry.Add(plot.Key);
                }
            }

            foreach (var plot in plotsToDry)
            {
                TileBase currentTile = tilemap.GetTile(plot);

                if (currentTile == wetGardenBedTile)
                {
                    tilemap.SetTile(plot, dryGardenBedTile);
                }
                else if (currentTile == wetDirtTile)
                {
                    tilemap.SetTile(plot, dryDirtTile);
                }

                _wateredPlots.Remove(plot);
            }
        }

        public void RefillWateringCan()
        {
            wateringCanUses = 5;
        }
    }
}