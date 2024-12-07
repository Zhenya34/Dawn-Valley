using TMPro;
using UI.SampleScene.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SampleScene.Shop
{
    public class SellingSlot : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI quantityText;
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private ItemDatabase itemDatabase;

        private Sprite _itemSprite;
        private int _quantity;
        private readonly Color _normalColor = Color.white;
        private readonly Color _selectedColor = new Color32(200, 200, 200, 255);

        private void Awake()
        {
            itemImage.preserveAspect = true;
            ClearSlot();
        }

        public bool IsEmpty()
        {
            return !_itemSprite;
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

        public Sprite GetItemSprite() => _itemSprite;

        public int GetQuantity() => _quantity;

        public void Select()
        {
            if (_itemSprite)
            {
                itemImage.color = _selectedColor;
            }
        }

        public void Deselect() => itemImage.color = _normalColor;

        public int GetTotalPrice()
        {
            Item selectedItem = itemDatabase.GetItemBySprite(_itemSprite);
            if (selectedItem != null)
            {
                return selectedItem.sellingPrice * _quantity;
            }
            return 0;
        }

        private void OnMouseDown() => inventoryManager.OnSellingSlotClicked(this);
    }
}