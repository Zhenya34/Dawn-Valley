using System.Collections.Generic;
using System.Linq;
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
            if (toolSwitcher.GetCurrentTool() != ToolSwitcher.ToolType.WateringCan) return;
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            var gridPosition = tilemap.WorldToCell(mouseWorldPos);
            var clickedTile = tilemap.GetTile(gridPosition);

            if (wateringCanUses <= 0) return;
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

        public bool IsGrowingOnWetTile(Vector3Int cellPosition)
        {
            var currentTile = tilemap.GetTile(cellPosition);
            return currentTile == wetDirtTile || currentTile == wetGardenBedTile;
        }

        private void UpdateWateringCan(Vector3Int gridPosition)
        {
            wateringCanUses--;
            _wateredPlots[gridPosition] = dayNightCycle.GetCurrentDay();
        }

        public void CheckForDryingPlots()
        {
            var plotsToDry = (from plot in _wateredPlots where dayNightCycle.GetCurrentDay() >= plot.Value + wetDuration select plot.Key).ToList();

            foreach (var plot in plotsToDry)
            {
                var currentTile = tilemap.GetTile(plot);

                if (currentTile == wetGardenBedTile)
                    tilemap.SetTile(plot, dryGardenBedTile);
                else if (currentTile == wetDirtTile) tilemap.SetTile(plot, dryDirtTile);

                _wateredPlots.Remove(plot);
            }
        }

        public void RefillWateringCan() => wateringCanUses = 5;
    }
}