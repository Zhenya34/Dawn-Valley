using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Player.Placement
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Tilemap tilemap;

        private Vector3 _lastPosition;
        public event Action OnClickedRightButton, OnExit;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                OnClickedRightButton?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnExit?.Invoke();
            }
        }

        public bool IsPointerOverUI()
            => EventSystem.current.IsPointerOverGameObject();

        public Vector3Int GetSelectedTileMapPosition()
        {
            var mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            var cellPosition = tilemap.WorldToCell(mouseWorldPos);
            return cellPosition;
        }
    }
}
