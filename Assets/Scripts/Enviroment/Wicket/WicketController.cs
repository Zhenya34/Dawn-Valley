using UnityEngine;

public class WicketController : MonoBehaviour
{
    private Collider2D _collider;
    private bool _isOpen;
    private bool _isHorizontal;

    [SerializeField] private Sprite _closedHorizontalSprite;
    [SerializeField] private Sprite _openHorizontalSprite;
    [SerializeField] private Sprite _closedVerticalSprite;
    [SerializeField] private Sprite _openVerticalSprite;

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

    private void OnMouseDown()
    {
        ToggleGate();
    }

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
            _spriteRenderer.sprite = _isOpen ? _openHorizontalSprite : _closedHorizontalSprite;
        }
        else
        {
            _spriteRenderer.sprite = _isOpen ? _openVerticalSprite : _closedVerticalSprite;
        }
    }
}