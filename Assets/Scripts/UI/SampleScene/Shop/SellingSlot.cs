using UnityEngine;
using UnityEngine.UI;

public class SellingSlot : MonoBehaviour
{
    private Image _itemImage;
    private Text _quantityText;

    private Sprite _itemSprite;
    private int _quantity;

    private void Awake()
    {
        _itemImage = transform.Find("ItemImage").GetComponent<Image>();
        _quantityText = transform.Find("QuantityText").GetComponent<Text>();

        if (_itemImage == null || _quantityText == null)
        {
            Debug.LogError("Dependencies are not set on the SellingSlot.");
            return;
        }

        ClearSlot();
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
}