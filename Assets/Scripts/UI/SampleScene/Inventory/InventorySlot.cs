using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.SampleScene.Inventory
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private Text quantityText;
        [SerializeField] private InventoryManager inventoryManager;

        private Sprite _itemSprite;
        private int _quantity;
        private readonly Color _normalColor = Color.white;
        private readonly Color _selectedColor = new Color32(200, 200, 200, 255);

        private void Awake()
        {
            itemImage.preserveAspect = true;
        }

        public bool IsEmpty()
        {
            return _itemSprite == null;
        }

        public void SetItem(Sprite sprite, int qty)
        {
            _itemSprite = sprite;
            _quantity = qty;
            itemImage.sprite = _itemSprite;
            itemImage.enabled = true;
            quantityText.text = _quantity.ToString();
            quantityText.enabled = true;
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
                quantityText.text = _quantity.ToString();
            }
        }

        public void ClearSlot()
        {
            _itemSprite = null;
            _quantity = 0;
            itemImage.enabled = false;
            quantityText.enabled = false;
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
                itemImage.color = _selectedColor;
            }
        }

        public void Deselect()
        {
            itemImage.color = _normalColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                inventoryManager.OnSlotLeftClicked(this);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                inventoryManager.OnSlotRightClicked(this);
            }
        }
    }
}