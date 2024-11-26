using UnityEngine;

namespace Player.Placement
{
    public class PreviewSystem : MonoBehaviour
    {
        [SerializeField] private GameObject cellIndicator;
        [SerializeField] private Grid grid;

        private SpriteRenderer _cellIndicatorRenderer;
        private GameObject _previewObject;

        private void Awake()
        {
            cellIndicator.SetActive(false);
            _cellIndicatorRenderer = cellIndicator.GetComponentInChildren<SpriteRenderer>();
        }

        public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
        {
            _previewObject = Instantiate(prefab);
            PreparePreview(_previewObject);
            PrepareCursor(size);
            cellIndicator.SetActive(true);
        }

        private void PrepareCursor(Vector2Int size)
        {
            if (size.x > 0 || size.y > 0)
            {
                cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
                _cellIndicatorRenderer.material.mainTextureScale = size;
            }
        }

        private void PreparePreview(GameObject previewObject)
        {
            SpriteRenderer[] spriteRenderers = previewObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            }
        }

        public void StopShowingPreview()
        {
            cellIndicator.SetActive(false);
            if (_previewObject != null)
            {
                Destroy(_previewObject);
                _previewObject = null;
            }
        }

        public void UpdatePosition(Vector3 position, bool validity)
        {
            if (_previewObject != null)
            {
                MovePreview(position);
                ApplyFeedbackToPreview(validity);
            }
            MoveCursor(position);
            ApplyFeedbackToCursor(validity);
        }

        private void ApplyFeedbackToPreview(bool validity)
        {
            Color c = validity ? new Color(1, 1, 1, 0.5f) : new Color(1, 0, 0, 0.5f);
            SpriteRenderer[] spriteRenderers = _previewObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = c;
            }
        }

        private void ApplyFeedbackToCursor(bool validity)
        {
            Color c = validity ? Color.white : Color.red;
            c.a = 0.5f;
            _cellIndicatorRenderer.material.color = c;
        }

        private void MoveCursor(Vector3 position)
        {
            Vector3Int gridPosition = grid.WorldToCell(position);
            Vector3 snappedPosition = grid.GetCellCenterWorld(gridPosition);
            cellIndicator.transform.position = new Vector3(
                snappedPosition.x,
                snappedPosition.y - 0.5f,
                snappedPosition.z);
        }

        private void MovePreview(Vector3 position)
        {
            Vector3Int gridPosition = grid.WorldToCell(position);
            Vector3 snappedPosition = grid.GetCellCenterWorld(gridPosition);
            _previewObject.transform.position = new Vector3(
                snappedPosition.x,
                snappedPosition.y - 0.5f,
                snappedPosition.z);
        }

        internal void StartShowingRemovePreview()
        {
            cellIndicator.SetActive(true);
            PrepareCursor(Vector2Int.one);
            ApplyFeedbackToCursor(false);
        }
    }
}