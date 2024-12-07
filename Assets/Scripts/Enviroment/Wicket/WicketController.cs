using UnityEngine;

namespace Enviroment.Wicket
{
    public class WicketController : MonoBehaviour
    {
        private Collider2D _collider;
        private bool _isOpen;
        private bool _isHorizontal;

        [SerializeField] private Sprite closedHorizontalSprite;
        [SerializeField] private Sprite openHorizontalSprite;
        [SerializeField] private Sprite closedVerticalSprite;
        [SerializeField] private Sprite openVerticalSprite;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _isOpen = false;
        }

        public void Initialize(bool isHorizontal)
        {
            _isHorizontal = isHorizontal;
            UpdateGateVisual();
        }

        private void OnMouseDown() => ToggleGate();

        private void ToggleGate()
        {
            _isOpen = !_isOpen;
            UpdateGateVisual();
            _collider.enabled = !_isOpen;
        }

        private void UpdateGateVisual()
        {
            if (_isHorizontal)
            {
                _spriteRenderer.sprite = _isOpen ? openHorizontalSprite : closedHorizontalSprite;
            }
            else
            {
                _spriteRenderer.sprite = _isOpen ? openVerticalSprite : closedVerticalSprite;
            }
        }
    }
}