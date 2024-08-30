using UnityEngine;
using UnityEngine.UI;

public class PetInventorySlot : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private InventoryManager _inventoryManager;

    private Sprite _itemSprite;
    private Color _normalColor = Color.white;
    private Color _selectedColor = new Color32(200, 200, 200, 255);

    private void Awake()
    {
        _itemImage.preserveAspect = true;
        ClearSlot();
    }

    public bool IsEmpty()
    {
        return _itemSprite == null;
    }

    public void SetItem(Sprite sprite)
    {
        _itemSprite = sprite;
        _itemImage.sprite = _itemSprite;
        _itemImage.enabled = true;
    }

    public void ClearSlot()
    {
        _itemSprite = null;
        _itemImage.enabled = false;
    }

    public Sprite GetItemSprite()
    {
        return _itemSprite;
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
        if (IsEmpty())
        {
            _inventoryManager.OnPetSlotClicked(this);
        }
        else
        {
            _inventoryManager.MovePetItemToInventory(this);
        }
    }
}