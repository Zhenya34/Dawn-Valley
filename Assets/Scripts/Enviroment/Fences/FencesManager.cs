using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class FencesManager : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private GameObject _horizontalConnectionPrefab;
    [SerializeField] private GameObject _verticalConnectionPrefab;
    [SerializeField] private GameObject _fencePrefab;
    [SerializeField] private ItemUsageManager _itemUsageManager;

    private readonly List<GameObject> _allFences = new();
    private bool _canPlace = false;
    private InventorySlot _currentSlot;

    private void Update()
    {
        if (_canPlace)
        {
            if (Input.GetMouseButtonDown(1) && _itemUsageManager.HasItemInInventory(Item.GlobalItemType.Fence))
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int tilePos = _tilemap.WorldToCell(mouseWorldPos);

                if (!IsFenceAtPosition(tilePos))
                {
                    GameObject newFence = Instantiate(_fencePrefab, _tilemap.GetCellCenterWorld(tilePos), Quaternion.identity);
                    _allFences.Add(newFence);

                    _itemUsageManager.UpdateCountOfItem(_currentSlot);

                    foreach (var fence in _allFences)
                    {
                        Vector3Int diff = tilePos - _tilemap.WorldToCell(fence.transform.position);
                        if (IsNeighbor(diff))
                        {
                            CreateConnections(fence, newFence);
                        }
                    }
                }
            }
        }
    }

    private bool IsNeighbor(Vector3Int diff)
    {
        return (Mathf.Abs(diff.x) == 1 && diff.y == 0) || (Mathf.Abs(diff.y) == 1 && diff.x == 0);
    }

    private bool IsFenceAtPosition(Vector3Int position)
    {
        foreach (var fence in _allFences)
        {
            Vector3Int fencePos = _tilemap.WorldToCell(fence.transform.position);
            if (fencePos == position)
            {
                return true;
            }
        }
        return false;
    }

    private void CreateConnections(GameObject fenceA, GameObject fenceB)
    {
        Vector3 midPoint = (fenceA.transform.position + fenceB.transform.position) / 2f;
        Vector3 direction = fenceB.transform.position - fenceA.transform.position;

        if (Mathf.Approximately(direction.x, 0f))
        {
            InstantiateFencePrefab(_verticalConnectionPrefab, midPoint, direction, true);
        }
        else if (Mathf.Approximately(direction.y, 0f))
        {
            InstantiateFencePrefab(_horizontalConnectionPrefab, midPoint, direction, false);
        }
    }

    private void InstantiateFencePrefab(GameObject _connectionPrefab, Vector3 midPoint, Vector3 direction, bool isVertical)
    {
        GameObject connection = Instantiate(_connectionPrefab, midPoint, Quaternion.identity);

        if (isVertical)
            connection.transform.up = direction;
        else
            connection.transform.right = direction;
    }

    public void SetFence(InventorySlot slot)
    {
        _currentSlot = slot;
    }

    public void AllowFencesPlacement()
    {
        _canPlace = true;
    }

    public void ForbidFencesPlacement()
    {
        _canPlace = false;
    }
}