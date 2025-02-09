using UI.SampleScene.Inventory;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Enviroment.Wicket
{
    public class WicketManager : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private GameObject horizontalGatePrefab;
        [SerializeField] private GameObject verticalGatePrefab;
        [SerializeField] private ItemUsageManager itemUsageManager;

        private bool _canPlace;
        private InventorySlot _currentSlot;

        private void Update()
        {
            if (!_canPlace) return;
            if (!Input.GetMouseButtonDown(1) ||
                !itemUsageManager.HasItemInInventory(Item.GlobalItemType.Wicket)) return;
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var tilePos = tilemap.WorldToCell(mouseWorldPos);
                
            if (IsTileEmpty(tilePos))
            {
                CreateGate(tilePos);
            }
        }

        private bool IsTileEmpty(Vector3Int position) => !tilemap.GetTile(position);

        private void CreateGate(Vector3Int tilePos)
        {
            GameObject newGate;
            var hasTopNeighbor = IsNeighbor(tilePos, Vector3Int.up);
            var hasBottomNeighbor = IsNeighbor(tilePos, Vector3Int.down);
            var hasLeftNeighbor = IsNeighbor(tilePos, Vector3Int.left);
            var hasRightNeighbor = IsNeighbor(tilePos, Vector3Int.right);

            if (hasTopNeighbor && hasBottomNeighbor)
            {
                newGate = Instantiate(verticalGatePrefab, tilemap.GetCellCenterWorld(tilePos), Quaternion.identity);
                InitializeGateController(false, newGate);
            }
            else if (hasLeftNeighbor && hasRightNeighbor)
            {
                newGate = Instantiate(horizontalGatePrefab, tilemap.GetCellCenterWorld(tilePos), Quaternion.identity);
                InitializeGateController(true, newGate);
            }
            else
            {
                newGate = Instantiate(horizontalGatePrefab, tilemap.GetCellCenterWorld(tilePos), Quaternion.identity);
                InitializeGateController(true, newGate);
            }

            itemUsageManager.UpdateCountOfItem(_currentSlot);
        }

        private void InitializeGateController(bool isHorizontal, GameObject newGate)
        {
            var gateController = newGate.AddComponent<WicketController>();
            gateController.Initialize(isHorizontal);
        }

        private bool IsNeighbor(Vector3Int position, Vector3Int direction)
        {
            var neighborPos = position + direction;
            return tilemap.GetTile(neighborPos);
        }

        public void SetWicket(InventorySlot slot) => _currentSlot = slot;

        public void AllowWicketsPlacement() => _canPlace = true;

        public void ForbidWicketsPlacement() => _canPlace = false;
    }
}