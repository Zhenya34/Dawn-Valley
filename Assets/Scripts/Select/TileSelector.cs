using UnityEngine;
using UnityEngine.Tilemaps;

namespace Select
{
    public class TileSelector : MonoBehaviour
    {
        [SerializeField] private GameObject framePrefab;
        [SerializeField] private Tilemap tilemap;

        private bool _canPlaceFrame = true;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (!_canPlaceFrame || !_camera) return;
            if (!Application.isFocused) return;
                
            var mouseScreenPos = Input.mousePosition;
                
            if (mouseScreenPos.x < 0 || mouseScreenPos.x > Screen.width ||
                mouseScreenPos.y < 0 || mouseScreenPos.y > Screen.height)
            {
                return;
            }
                
            var mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            var cellPosition = tilemap.WorldToCell(mouseWorldPos);

            if (!tilemap.HasTile(cellPosition)) return;
            var tileCenter = tilemap.GetCellCenterWorld(cellPosition);

            var cellSize = tilemap.cellSize;
            var offsetY = cellSize.y / 2f;
            framePrefab.transform.position = new Vector3(tileCenter.x, tileCenter.y - offsetY, tileCenter.z);
        }

        public void AllowFramePlacement() => _canPlaceFrame = true;

        public void ProhibitFramePlacement() => _canPlaceFrame = false;
    }
}