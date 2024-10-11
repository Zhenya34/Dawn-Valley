using UnityEngine;
using UnityEngine.Tilemaps;

public class WicketManager : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private GameObject _horizontalGatePrefab;
    [SerializeField] private GameObject _verticalGatePrefab;
    [SerializeField] private ItemUsageManager _itemUsageManager;

    private bool _canPlace = false;
    private InventorySlot _currentSlot;

    private void Update()
    {
        if (_canPlace)
        {
            if (Input.GetMouseButtonDown(1) && _itemUsageManager.HasItemInInventory(Item.GlobalItemType.Wicket))
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int tilePos = _tilemap.WorldToCell(mouseWorldPos);
                
                if (IsTileEmpty(tilePos))
                {
                    CreateGate(tilePos);
                }
            }
        }
    }

    private bool IsTileEmpty(Vector3Int position)
    {
        return _tilemap.GetTile(position) == null;
    }

    private void CreateGate(Vector3Int tilePos)
    {
        GameObject newGate;
        bool hasTopNeighbor = IsNeighbor(tilePos, Vector3Int.up);
        bool hasBottomNeighbor = IsNeighbor(tilePos, Vector3Int.down);
        bool hasLeftNeighbor = IsNeighbor(tilePos, Vector3Int.left);
        bool hasRightNeighbor = IsNeighbor(tilePos, Vector3Int.right);

        if (hasTopNeighbor && hasBottomNeighbor)
        {
            newGate = Instantiate(_verticalGatePrefab, _tilemap.GetCellCenterWorld(tilePos), Quaternion.identity);
            InitializeGateController(false, newGate);
        }
        else if (hasLeftNeighbor && hasRightNeighbor)
        {
            newGate = Instantiate(_horizontalGatePrefab, _tilemap.GetCellCenterWorld(tilePos), Quaternion.identity);
            InitializeGateController(true, newGate);
        }
        else
        {
            newGate = Instantiate(_horizontalGatePrefab, _tilemap.GetCellCenterWorld(tilePos), Quaternion.identity);
            InitializeGateController(true, newGate);
        }

        _itemUsageManager.UpdateCountOfItem(_currentSlot);
    }

    private void InitializeGateController(bool isHorizontal, GameObject newGate)
    {
        WicketController gateController = newGate.AddComponent<WicketController>();
        gateController.Initialize(isHorizontal);
    }

    private bool IsNeighbor(Vector3Int position, Vector3Int direction)
    {
        Vector3Int neighborPos = position + direction;
        return _tilemap.GetTile(neighborPos) != null;
    }

    public void SetWicket(InventorySlot slot)
    {
        _currentSlot = slot;
    }

    public void AllowWicketsPlacement()
    {
        _canPlace = true;
    }

    public void ForbidWicketsPlacement()
    {
        _canPlace = false;
    }
}