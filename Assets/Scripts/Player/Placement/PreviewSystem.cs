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
            _cellIndicatorRenderer = cellIndicator.GetComponentInChildren<SpriteRenderer>();
        }

        public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
        {
            _previewObject = Instantiate(prefab);
            PreparePreview(_previewObject);
            PrepareCursor(size);
        }

        private void PrepareCursor(Vector2Int size)
        {
            if (size.x > 0 || size.y > 0) cellIndicator.transform.localScale = new Vector3(size.x, size.y, 1);
        }

        private static void PreparePreview(GameObject previewObject)
        {
            var spriteRenderers = previewObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            }
        }

        public void StopShowingPreview()
        {
            if (!_previewObject) return;
            Destroy(_previewObject);
            _previewObject = null;
        }

        public void UpdatePosition(Vector3 position, bool validity)
        {
            if (_previewObject)
            {
                MovePreview(position);
                ApplyFeedbackToPreview(validity);
            }
            MoveCursor(position);
            ApplyFeedbackToCursor(validity);
        }

        private void ApplyFeedbackToPreview(bool validity)
        {
            var c = validity ? new Color(1, 1, 1, 0.5f) : new Color(1, 0, 0, 0.5f);
            var spriteRenderers = _previewObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = c;
            }
        }
        
        private void ResetCursorSize() => cellIndicator.transform.localScale = Vector3.one;

        private void ApplyFeedbackToCursor(bool validity)
        {
            Color c;
            if (validity)
            {
                c = Color.white;
                c.a = 1f;
            }
            else
            {
                c = Color.red;
                c.a = 0.5f;
            }
            _cellIndicatorRenderer.material.color = c;
        }

        private void MoveCursor(Vector3 position)
        {
            var gridPosition = grid.WorldToCell(position);
            var snappedPosition = grid.GetCellCenterWorld(gridPosition);
            cellIndicator.transform.position = new Vector3(
                snappedPosition.x,
                snappedPosition.y - 0.5f,
                snappedPosition.z);
        }

        private void MovePreview(Vector3 position)
        {
            var gridPosition = grid.WorldToCell(position);
            var snappedPosition = grid.GetCellCenterWorld(gridPosition);
            _previewObject.transform.position = new Vector3(
                snappedPosition.x,
                snappedPosition.y - 0.5f,
                snappedPosition.z);
        }

        internal void StartShowingRemovePreview()
        {
            PrepareCursor(Vector2Int.one);
            ApplyFeedbackToCursor(false);
        }

        internal void StopShowingRemovePreview()
        {
            ApplyFeedbackToCursor(true);
            ResetCursorSize();
        } 
    }
}