using System.Collections.Generic;
using Enviroment.Plants;
using Enviroment.Time;
using Select;
using UI.SampleScene;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Player.ToolsLogic
{
    public class HoeLogic : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase grassTile;
        [SerializeField] private TileBase dirtTile;
        [SerializeField] private TileBase gardenBed;
        [SerializeField] private TileBase wetGardenBed;
        [SerializeField] private TileBase wetDirtTile;
        [SerializeField] private ToolSwitcher toolSwitcher;
        [SerializeField] private DayNightCycle dayNightCycle;
        [SerializeField] private PlayerImpactRadius playerImpactRadius;
        [SerializeField] private int daysBeforeDirtBecomeGrass;

        private readonly Dictionary<Vector3Int, int> _tilledSoilTiles = new();
        private enum ClickType { None, LeftClick, RightClick }
        private ClickType _lastClick = ClickType.None;

        private void RegisterMouseClick()
        {
            if (toolSwitcher.GetCurrentTool() != ToolSwitcher.ToolType.Hoe) return;
            if (Input.GetMouseButton(0))
            {
                _lastClick = ClickType.LeftClick;
            }
            else if (Input.GetMouseButton(1))
            {
                _lastClick = ClickType.RightClick;
            }
        }

        private void Update()
        {
            RegisterMouseClick();
            CheckForGrassRegrowth();
        }

        public void HandleHoeLogic()
        {
            if (toolSwitcher.GetCurrentTool() != ToolSwitcher.ToolType.Hoe) return;
            HandleTileClick(out var gridPosition, out var clickedTile);

            var playerPosition = transform.position;
            var tileWorldPosition = tilemap.CellToWorld(gridPosition);

            var distanceToTile = Vector3.Distance(playerPosition, tileWorldPosition);

            if (distanceToTile > playerImpactRadius.GetToolDistanceValue())
            {
                return;
            }

            if (_lastClick == ClickType.RightClick)
            {
                if (clickedTile == grassTile || clickedTile == dirtTile)
                {
                    tilemap.SetTile(gridPosition, gardenBed);
                    _tilledSoilTiles.Remove(gridPosition);
                }
                else if (clickedTile == wetDirtTile)
                {
                    tilemap.SetTile(gridPosition, wetGardenBed);
                    _tilledSoilTiles.Remove(gridPosition);
                }
            }
            if (_lastClick == ClickType.LeftClick)
            {
                if (clickedTile == gardenBed)
                {
                    tilemap.SetTile(gridPosition, dirtTile);
                    _tilledSoilTiles[gridPosition] = dayNightCycle.GetCurrentDay();
                    RemovePlantIfExists(gridPosition);
                }
                else if (clickedTile == wetGardenBed)
                {
                    tilemap.SetTile(gridPosition, wetDirtTile);
                    _tilledSoilTiles[gridPosition] = dayNightCycle.GetCurrentDay();
                    RemovePlantIfExists(gridPosition);
                }
            }

            _lastClick = ClickType.None;
            CheckForGrassRegrowth();
        }

        private static void RemovePlantIfExists(Vector3Int gridPosition)
        {
            var plantingSystem = FindObjectOfType<Planting>();
            if (!plantingSystem || !plantingSystem.IsPlantAtTile(gridPosition)) return;
            var plant = plantingSystem.GetPlantAtTile(gridPosition);
            if (plant)
            {
                Destroy(plant.gameObject);
            }
        }

        private void CheckForGrassRegrowth()
        {
            List<Vector3Int> toRemove = new();
            List<Vector3Int> grassReady = new();

            foreach (var tilledTile in _tilledSoilTiles)
            {
                var gridPosition = tilledTile.Key;
                var dayTilled = tilledTile.Value;
                var daysPassed = dayNightCycle.GetCurrentDay() - dayTilled;

                var currentTile = tilemap.GetTile(gridPosition);

                if (currentTile == wetGardenBed && daysPassed >= 1)
                {
                    tilemap.SetTile(gridPosition, dirtTile);
                    _tilledSoilTiles[gridPosition] = dayNightCycle.GetCurrentDay();
                }
                else if (currentTile == dirtTile)
                {
                    if (daysPassed >= daysBeforeDirtBecomeGrass)
                    {
                        grassReady.Add(gridPosition);
                    }
                }
            }

            foreach (var gridPosition in grassReady)
            {
                tilemap.SetTile(gridPosition, grassTile);
                toRemove.Add(gridPosition);
            }

            foreach (var gridPosition in toRemove)
            {
                _tilledSoilTiles.Remove(gridPosition);
            }
        }

        private void HandleTileClick(out Vector3Int gridPosition, out TileBase clickedTile)
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            gridPosition = tilemap.WorldToCell(mouseWorldPos);
            clickedTile = tilemap.GetTile(gridPosition);
        }
    }
}