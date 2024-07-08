using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class FencesManager : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private GameObject _horizontalConnectionPrefab;
    [SerializeField] private GameObject _verticalConnectionPrefab;
    [SerializeField] private GameObject _fencePrefab;

    private readonly List<GameObject> _allFences = new();

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilePos = _tilemap.WorldToCell(mouseWorldPos);

            if (!IsFenceAtPosition(tilePos))
            {
                GameObject newFence = Instantiate(_fencePrefab, _tilemap.GetCellCenterWorld(tilePos), Quaternion.identity);
                _allFences.Add(newFence);

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
            GameObject connection = Instantiate(_verticalConnectionPrefab, midPoint, Quaternion.identity);
            connection.transform.up = direction;
        }
        else if (Mathf.Approximately(direction.y, 0f))
        {
            GameObject connection = Instantiate(_horizontalConnectionPrefab, midPoint, Quaternion.identity);
            connection.transform.right = direction;
        }
    }
}