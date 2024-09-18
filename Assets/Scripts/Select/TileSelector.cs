using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    [SerializeField] private GameObject _framePrefab;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Tilemap _tilemap;

    private bool canPlaceFrame = true;

    private void Update()
    {
        if (canPlaceFrame)
        {
            Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            Vector3Int cellPosition = _tilemap.WorldToCell(mouseWorldPos);

            if (_tilemap.HasTile(cellPosition))
            {
                Vector3 tileCenter = _tilemap.GetCellCenterWorld(cellPosition);

                Vector3 cellSize = _tilemap.cellSize;
                float offsetY = cellSize.y / 2f;
                _framePrefab.transform.position = new Vector3(tileCenter.x, tileCenter.y - offsetY, tileCenter.z);
            }
        }
    }

    public void AllowFramePlacement()
    {
        canPlaceFrame = true;
    }

    public void ProhibitFramePlacement()
    {
        canPlaceFrame = false;
    }
}