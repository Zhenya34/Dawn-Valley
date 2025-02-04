using System.Collections.Generic;
using System.Linq;
using UI.SampleScene.Inventory;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Enviroment.Fences
{
    public class FencesManager : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private GameObject horizontalConnectionPrefab;
        [SerializeField] private GameObject verticalConnectionPrefab;
        [SerializeField] private GameObject fencePrefab;
        [SerializeField] private ItemUsageManager itemUsageManager;
        [SerializeField] private InventoryManager inventoryManager;

        private readonly List<GameObject> _allFences = new();
        private readonly List<GameObject> _allConnections = new();
        private bool _canPlace;
        private bool _isRemovingMode;
        private InventorySlot _currentSlot;
        private float _holdTimer;
        private const float HoldThreshold = 0.5f;

        private void Update()
        {
            if (_canPlace) HandleFencePlacement();

            if (_isRemovingMode) HandleFenceRemoving();
        }

        private void HandleFencePlacement()
        {
            if (Input.GetMouseButtonDown(1) && itemUsageManager.HasItemInInventory(Item.GlobalItemType.Fence))
            {
                if (Camera.main)
                {
                    Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3Int tilePos = tilemap.WorldToCell(mouseWorldPos);
                    
                    Vector3 adjustedPos = tilemap.GetCellCenterWorld(tilePos);
                    adjustedPos.z = 0f;

                    if (!IsFenceAtPosition(tilePos))
                    {
                        GameObject newFence = Instantiate(fencePrefab, adjustedPos, Quaternion.identity);
                        _allFences.Add(newFence);

                        itemUsageManager.UpdateCountOfItem(_currentSlot);

                        foreach (var fence in _allFences)
                        {
                            Vector3Int diff = tilePos - tilemap.WorldToCell(fence.transform.position);
                            if (IsNeighbor(diff))
                            {
                                CreateConnections(fence, newFence);
                            }
                        }
                    }
                }
            }
        }
        
        private void HandleFenceRemoving()
        {
            if (Input.GetMouseButton(0))
            {
                _holdTimer += UnityEngine.Time.deltaTime;

                if (_holdTimer >= HoldThreshold)
                {
                    if (Camera.main)
                    {
                        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Vector3Int tilePos = tilemap.WorldToCell(mouseWorldPos);

                        if (IsFenceAtPosition(tilePos))
                        {
                            RemoveFenceAtPosition(tilePos);
                        }
                    }

                    _holdTimer = 0.0f;
                }
            }
            else
            {
                _holdTimer = 0.0f;
            }
        }
        
        private void RemoveFenceAtPosition(Vector3Int tilePos)
        {
            GameObject fenceToRemove = (from fence in _allFences let fencePos = tilemap.WorldToCell(fence.transform.position) where fencePos == tilePos select fence).FirstOrDefault();

            if (!fenceToRemove) return;
            inventoryManager.AddItem("ShopItem Fence", 1);
            _allFences.Remove(fenceToRemove);
            RemoveConnections(fenceToRemove);
            Destroy(fenceToRemove);
        }

        private static bool IsNeighbor(Vector3Int diff)
        {
            return (Mathf.Abs(diff.x) == 1 && diff.y == 0) || (Mathf.Abs(diff.y) == 1 && diff.x == 0);
        }

        private bool IsFenceAtPosition(Vector3Int position)
        {
            return _allFences.Select(fence => tilemap.WorldToCell(fence.transform.position)).Any(fencePos => fencePos == position);
        }

        private void CreateConnections(GameObject fenceA, GameObject fenceB)
        {
            var midPoint = (fenceA.transform.position + fenceB.transform.position) / 2f;
            var direction = fenceB.transform.position - fenceA.transform.position;

            GameObject connection = null;

            if (Mathf.Approximately(direction.x, 0f))
            {
                connection = Instantiate(verticalConnectionPrefab, midPoint, Quaternion.identity);
                connection.transform.up = direction;
            }
            else if (Mathf.Approximately(direction.y, 0f))
            {
                connection = Instantiate(horizontalConnectionPrefab, midPoint, Quaternion.identity);
                connection.transform.right = direction;
            }

            if (connection)
            {
                _allConnections.Add(connection);
            }
        }
        
        private void RemoveConnections(GameObject fence)
        {
            List<GameObject> connectionsToRemove = new();

            foreach (var connection in _allConnections)
            {
                connectionsToRemove.AddRange(from otherFence in _allFences where otherFence != fence select (fence.transform.position + otherFence.transform.position) / 2f into expectedMidPoint where Vector3.Distance(connection.transform.position, expectedMidPoint) < 0.1f select connection);
            }

            foreach (var connection in connectionsToRemove)
            {
                _allConnections.Remove(connection);
                Destroy(connection);
            }
        }
        
        public void StartRemoving() => _isRemovingMode = true;

        public void StopRemoving() => _isRemovingMode = false;

        public void SetFence(InventorySlot slot) => _currentSlot = slot;

        public void AllowFencesPlacement() => _canPlace = true;

        public void ForbidFencesPlacement() => _canPlace = false;
    }
}