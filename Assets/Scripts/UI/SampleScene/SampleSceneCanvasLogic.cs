using Enviroment.Time;
using UI.SampleScene.Shop;
using UnityEngine;
using Zenject;

namespace UI.SampleScene
{
    public class SampleSceneCanvasLogic : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        
        private DayNightCycle _dayNightCycle;
        private UIManager.UIManager _uiManager;
        private SellingItemsLogic _sellingItemsLogic;
        private UIElements _uiElements;
        
        [Inject]
        private void Construct(UIManager.UIManager uiManager, SellingItemsLogic sellingItemsLogic, DayNightCycle dayNightCycle, UIElements uiElements)
        {
            _uiManager = uiManager;
            _sellingItemsLogic = sellingItemsLogic;
            _dayNightCycle = dayNightCycle;
            _uiElements = uiElements;
        }

        private void Awake()
        {
            if (_dayNightCycle)
                _dayNightCycle.ResumeGame();
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.I)) return;
            if (!_uiElements.InventoryPanel) return;
            if (!_uiElements.InventoryPanel.activeSelf)
                OpenInventory();
            else
                CloseInventory();
        }

        public void OpenPausePanel()
        {
            if (_uiManager.IsUIActive()) return;
            _uiElements.PausePanel.SetActive(true);
            _uiElements.PauseButton.SetActive(false);
            _uiManager.ActivateUI();
            if (_dayNightCycle)
                _dayNightCycle.PauseGame();
        }

        public void ClosePausePanel()
        {
            _uiElements.PausePanel.SetActive(false);
            _uiElements.PauseButton.SetActive(true);
            _uiManager.DeactivateUI();
            if (_dayNightCycle)
                _dayNightCycle.ResumeGame();
        }

        public void OpenSettingsPanel()
        {
            _uiElements.PausePanel.SetActive(false);
            _uiElements.SettingsPanel.SetActive(true);
            if (_dayNightCycle)
                _dayNightCycle.PauseGame();
        }

        public void CloseSettingsPanel()
        {
            _uiElements.SettingsPanel.SetActive(false);
            _uiElements.PausePanel.SetActive(true);
        }

        public void OpenStatsPanel()
        {
            _uiElements.StatsPanel.SetActive(true);
            _uiElements.PausePanel.SetActive(false);
            if (_dayNightCycle)
                _dayNightCycle.PauseGame();
        }

        public void CloseStatsPanel()
        {
            _uiElements.StatsPanel.SetActive(false);
            _uiElements.PausePanel.SetActive(true);
        }

        private void OpenInventory()
        {
            if (_uiManager.IsUIActive()) return;
            _uiElements.InventoryPanel.SetActive(true);
            _uiElements.PauseButton.SetActive(false);
            _uiManager.ActivateUI();
        }

        public void CloseInventory()
        {
            _uiElements.InventoryPanel.SetActive(false);
            _uiElements.PauseButton.SetActive(true);
            _uiManager.DeactivateUI();

            if (_sellingItemsLogic.PanelIsActive())
            {
                _sellingItemsLogic.CloseShop();
            }
        }

        public void CloseUpgradePanel()
        {
            _uiElements.UpgradePanel.SetActive(false);
            _uiElements.PauseButton.SetActive(true);
            _uiManager.DeactivateUI();
        }

        public void CloseShopPanel()
        {
            _uiElements.ShopPanel.SetActive(false);
            _uiElements.PauseButton.SetActive(true);
            _uiManager.DeactivateUI();
        }

        public void CloseSellingPanel()
        {
            _sellingItemsLogic.CloseShop();
            _uiElements.PauseButton.SetActive(true);
            _uiManager.DeactivateUI();
        }

        public void SwitchOffPauseButton() => _uiElements.PauseButton.SetActive(false);

        public void OpenMainMenu() => UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}