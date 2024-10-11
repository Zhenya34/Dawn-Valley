using UnityEngine;
using UnityEngine.UI;

public class SellingItemsLogic : MonoBehaviour
{
    [SerializeField] private SellingSlot _shopSlot;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private PlayerCoinsWallet _playerWallet;
    [SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField] private RectTransform _inventoryRectTransform;
    [SerializeField] private SampleSceneCanvasLogic _sampleSceneCanvasLogic;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private float _shopRadius;
    
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _sellingPanel;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _inventoryExitButton;

    private void Start()
    {
        _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        _cancelButton.onClick.AddListener(OnCancelButtonClicked);
    }

    private void OnMouseDown()
    {
        if (!_uiManager.IsUIActive())
        {
            float distance = Vector3.Distance(_player.transform.position, transform.position);
            if (distance <= _shopRadius)
            {
                _uiManager.ActivateUI();
                OpenShop();
            }
        }
    }

    public bool PanelIsActive()
    {
        return _sellingPanel.activeSelf;
    }

    private void OpenShop()
    {
        _sampleSceneCanvasLogic.SwitchOffPauseButton();
        Vector3 newPosition = _inventoryRectTransform.transform.localPosition;
        newPosition.y = -150;
        _inventoryRectTransform.localPosition = newPosition;

        _inventoryPanel.SetActive(true);
        _sellingPanel.SetActive(true);
        _inventoryExitButton.SetActive(false);
    }

    public void CloseShop()
    {
        Vector3 newPosition = _inventoryRectTransform.transform.localPosition;
        newPosition.y = -30;
        _inventoryRectTransform.localPosition = newPosition;

        _inventoryPanel.SetActive(false);
        _sellingPanel.SetActive(false);
        _inventoryExitButton.SetActive(true);
    }

    public void AddToShopSlot(Sprite sprite, int quantity)
    {
        _shopSlot.SetItem(sprite, quantity);
    }

    private void ConfirmSale()
    {
        if (_shopSlot.GetItemSprite() != null)
        {
            int totalPrice = _shopSlot.GetTotalPrice();
            _playerWallet.AddCoins(totalPrice);
            _shopSlot.ClearSlot();
        }
    }

    private void CancelSale()
    {
        if (_shopSlot.GetItemSprite() != null)
        {
            _inventoryManager.MoveItemToInventory(_shopSlot);
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