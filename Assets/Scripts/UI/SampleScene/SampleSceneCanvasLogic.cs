using UnityEngine;

public class SampleSceneCanvasLogic : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private SellingItemsLogic _sellingItemsLogic;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _statsPanel;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _sellingPanel;
    [SerializeField] private string _sceneName;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!_inventoryPanel.activeSelf)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }

    public void OpenPausePanel()
    {
        if (!_uiManager.IsUIActive())
        {
            _pausePanel.SetActive(true);
            _pauseButton.SetActive(false);
            _uiManager.ActivateUI();
        }
    }

    public void ClosePausePanel()
    {
        _pausePanel.SetActive(false);
        _pauseButton.SetActive(true);
        _uiManager.DeactivateUI();
    }

    public void OpenSettingsPanel()
    {
        _pausePanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        _settingsPanel.SetActive(false);
        _pausePanel.SetActive(true);
    }

    public void OpenStatsPanel()
    {
        _statsPanel.SetActive(true);
        _pausePanel.SetActive(false);
    }

    public void CloseStatsPanel()
    {
        _statsPanel.SetActive(false);
        _pausePanel.SetActive(true);
    }

    public void OpenInventory()
    {
        if (!_uiManager.IsUIActive())
        {
            _inventoryPanel.SetActive(true);
            _pauseButton.SetActive(false);
            _uiManager.ActivateUI();
        }
    }

    public void CloseInventory()
    {
        _inventoryPanel.SetActive(false);
        _pauseButton.SetActive(true);
        _uiManager.DeactivateUI();

        if (_sellingItemsLogic.PanelIsActive())
        {
            _sellingItemsLogic.CloseShop();
        }
    }

    public void CloseUpgradePanel()
    {
        _upgradePanel.SetActive(false);
        _pauseButton.SetActive(true);
        _uiManager.DeactivateUI();
    }

    public void CloseShopPanel()
    {
        _shopPanel.SetActive(false);
        _pauseButton.SetActive(true);
        _uiManager.DeactivateUI();
    }

    public void CloseSellingPanel()
    {
        _sellingItemsLogic.CloseShop();
        _pauseButton.SetActive(true);
        _uiManager.DeactivateUI();
    }

    public void SwitchOffPauseButton()
    {
        _pauseButton.SetActive(false);
    }

    public void OpenMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName);
    }
}