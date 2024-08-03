using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private Text _quantityText;
    [SerializeField] private InventoryManager _inventoryManager;

    private Sprite _itemSprite;
    private int _quantity;
    private Color _normalColor = Color.white;
    private Color _selectedColor = new Color32(200, 200, 200, 255);

    private void Awake()
    {
        _itemImage.preserveAspect = true;
    }

    public bool IsEmpty()
    {
        return _itemSprite == null;
    }

    public void SetItem(Sprite sprite, int qty)
    {
        _itemSprite = sprite;
        _quantity = qty;
        _itemImage.sprite = _itemSprite;
        _itemImage.enabled = true;
        _quantityText.text = _quantity.ToString();
        _quantityText.enabled = true;
    }

    public void UpdateQuantity(int qty)
    {
        _quantity = qty;
        if (_quantity <= 0)
        {
            ClearSlot();
        }
        else
        {
            _quantityText.text = _quantity.ToString();
        }
    }

    public void ClearSlot()
    {
        _itemSprite = null;
        _quantity = 0;
        _itemImage.enabled = false;
        _quantityText.enabled = false;
    }

    public Sprite GetItemSprite()
    {
        return _itemSprite;
    }

    public int GetQuantity()
    {
        return _quantity;
    }

    public void Select()
    {
        if (_itemSprite != null)
        {
            _itemImage.color = _selectedColor;
        }
    }

    public void Deselect()
    {
        _itemImage.color = _normalColor;
    }

    private void OnMouseDown()
    {
        _inventoryManager.OnSlotClicked(this);
    }
}