using UnityEngine;
using UnityEngine.Tilemaps;

namespace Select
{
    public class TileSelector : MonoBehaviour
    {
        [SerializeField] private GameObject framePrefab;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Tilemap tilemap;

        private bool _canPlaceFrame = true;

        private void Update()
        {
            if (_canPlaceFrame)
            {
                Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0;
                Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

                if (tilemap.HasTile(cellPosition))
                {
                    Vector3 tileCenter = tilemap.GetCellCenterWorld(cellPosition);

                    Vector3 cellSize = tilemap.cellSize;
                    float offsetY = cellSize.y / 2f;
                    framePrefab.transform.position = new Vector3(tileCenter.x, tileCenter.y - offsetY, tileCenter.z);
                }
            }
        }

        public void AllowFramePlacement()
        {
            _canPlaceFrame = true;
        }

        public void ProhibitFramePlacement()
        {
            _canPlaceFrame = false;
        }
    }
}