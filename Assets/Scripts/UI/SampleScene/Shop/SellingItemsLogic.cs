using UI.SampleScene.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SampleScene.Shop
{
    public class SellingItemsLogic : MonoBehaviour
    {
        [SerializeField] private SellingSlot shopSlot;
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private PlayerCoinsWallet playerWallet;
        [SerializeField] private ItemDatabase itemDatabase;
        [SerializeField] private RectTransform inventoryRectTransform;
        [SerializeField] private SampleSceneCanvasLogic sampleSceneCanvasLogic;
        [SerializeField] private UIManager.UIManager uiManager;
        [SerializeField] private float shopRadius;
    
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private GameObject sellingPanel;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject inventoryExitButton;

        private void Start()
        {
            confirmButton.onClick.AddListener(OnConfirmButtonClicked);
            cancelButton.onClick.AddListener(OnCancelButtonClicked);
        }

        private void OnMouseDown()
        {
            if (!uiManager.IsUIActive())
            {
                float distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance <= shopRadius)
                {
                    uiManager.ActivateUI();
                    OpenShop();
                }
            }
        }

        public bool PanelIsActive()
        {
            return sellingPanel.activeSelf;
        }

        private void OpenShop()
        {
            sampleSceneCanvasLogic.SwitchOffPauseButton();
            Vector3 newPosition = inventoryRectTransform.transform.localPosition;
            newPosition.y = -150;
            inventoryRectTransform.localPosition = newPosition;

            inventoryPanel.SetActive(true);
            sellingPanel.SetActive(true);
            inventoryExitButton.SetActive(false);
        }

        public void CloseShop()
        {
            Vector3 newPosition = inventoryRectTransform.transform.localPosition;
            newPosition.y = -30;
            inventoryRectTransform.localPosition = newPosition;

            inventoryPanel.SetActive(false);
            sellingPanel.SetActive(false);
            inventoryExitButton.SetActive(true);
        }

        public void AddToShopSlot(Sprite sprite, int quantity)
        {
            shopSlot.SetItem(sprite, quantity);
        }

        private void ConfirmSale()
        {
            if (shopSlot.GetItemSprite() != null)
            {
                int totalPrice = shopSlot.GetTotalPrice();
                playerWallet.AddCoins(totalPrice);
                shopSlot.ClearSlot();
            }
        }

        private void CancelSale()
        {
            if (shopSlot.GetItemSprite() != null)
            {
                inventoryManager.MoveItemToInventory(shopSlot);
            }
        }

        private void OnConfirmButtonClicked()
        {
            ConfirmSale();
        }

        private void OnCancelButtonClicked()
        {
            CancelSale();
        }
    }
}