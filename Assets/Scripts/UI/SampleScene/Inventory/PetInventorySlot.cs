using UnityEngine;
using UnityEngine.UI;

namespace UI.SampleScene.Inventory
{
    public class PetInventorySlot : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private InventoryManager inventoryManager;

        private Sprite _itemSprite;
        private readonly Color _normalColor = Color.white;
        private readonly Color _selectedColor = new Color32(200, 200, 200, 255);

        private void Awake()
        {
            itemImage.preserveAspect = true;
            ClearSlot();
        }

        public bool IsEmpty() => !_itemSprite;

        public void SetItem(Sprite sprite)
        {
            _itemSprite = sprite;
            itemImage.sprite = _itemSprite;
            itemImage.enabled = true;
        }

        public void ClearSlot()
        {
            _itemSprite = null;
            itemImage.enabled = false;
        }

        public Sprite GetItemSprite() => _itemSprite;

        public void Select()
        {
            if (_itemSprite)
            {
                itemImage.color = _selectedColor;
            }
        }

        public void Deselect() => itemImage.color = _normalColor;

        private void OnMouseDown()
        {
            if (IsEmpty())
            {
                inventoryManager.OnPetSlotClicked(this);
            }
            else
            {
                inventoryManager.MovePetItemToInventory(this);
            }
        }
    }
}