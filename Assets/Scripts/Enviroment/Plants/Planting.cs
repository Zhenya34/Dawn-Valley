using System.Collections.Generic;
using Enviroment.Time;
using Player;
using UI.SampleScene.Inventory;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Enviroment.Plants
{
    public class Planting : MonoBehaviour
    {
        [SerializeField] private PlayerAnimation playerAnim;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase gardenBedTile;
        [SerializeField] private TileBase wetGardenBedTile;
        [SerializeField] private ItemUsageManager itemUsageManager;
        [SerializeField] private DayNightCycle dayNightCycle;

        private Plant _currentPlant;
        private InventorySlot _currentSlot;
        private bool _canPlant = true;

        private readonly Dictionary<Vector3Int, bool> _occupiedTiles = new();
        private readonly Dictionary<Vector3Int, PlantsGrowth> _plantsByTile = new();

        private void Update()
        {
            if (!_canPlant) return;
            if (!Input.GetMouseButtonDown(1) || !playerAnim.GetToolsUsingValue() || !_currentPlant) return;
            var mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            var cellPosition = tilemap.WorldToCell(mouseWorldPos);

            if (tilemap.GetTile(cellPosition) != gardenBedTile && (tilemap.GetTile(cellPosition) != wetGardenBedTile ||
                                                                   (_occupiedTiles.ContainsKey(cellPosition) &&
                                                                    _occupiedTiles[cellPosition]))) return;
            var tileCenter = tilemap.GetCellCenterWorld(cellPosition);

            Instantiate(_currentPlant.plantPrefab, tileCenter, Quaternion.identity);

            _occupiedTiles[cellPosition] = true;
            itemUsageManager.UpdateCountOfItem(_currentSlot);
        }

        public void RegisterPlant(Vector3Int cellPosition, PlantsGrowth plant)
        {
            _plantsByTile.TryAdd(cellPosition, plant);
        }

        public void UnregisterPlant(Vector3Int cellPosition)
        {
            if (_plantsByTile.ContainsKey(cellPosition))
            {
                _plantsByTile.Remove(cellPosition);
            }
        }

        public bool IsPlantAtTile(Vector3Int cellPosition) => _plantsByTile.ContainsKey(cellPosition);

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

        public Vector3Int GetCellPosition(Vector3 worldPosition) => tilemap.WorldToCell(worldPosition);

        public void PlantSeed(Plant plant, InventorySlot slot)
        {
            _currentPlant = plant;
            _currentSlot = slot;
        }

        public void AllowPlanting() => _canPlant = true;

        public void ForbidPlanting() => _canPlant = false;
    }
}