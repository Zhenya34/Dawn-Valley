using Enviroment.Time;
using UI.SampleScene.Shop;
using UnityEngine;

namespace UI.SampleScene
{
    public class SampleSceneCanvasLogic : MonoBehaviour
    {
        [SerializeField] private UIManager.UIManager uiManager;
        [SerializeField] private SellingItemsLogic sellingItemsLogic;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject statsPanel;
        [SerializeField] private GameObject pauseButton;
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private GameObject upgradePanel;
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private GameObject sellingPanel;
        [SerializeField] private string sceneName;
        [SerializeField] private DayNightCycle dayNightCycle;

        private void Awake()
        {
            dayNightCycle.ResumeGame();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (!inventoryPanel.activeSelf)
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
            if (!uiManager.IsUIActive())
            {
                pausePanel.SetActive(true);
                pauseButton.SetActive(false);
                uiManager.ActivateUI();
                dayNightCycle.PauseGame();
            }
        }

        public void ClosePausePanel()
        {
            pausePanel.SetActive(false);
            pauseButton.SetActive(true);
            uiManager.DeactivateUI();
            dayNightCycle.ResumeGame();
        }

        public void OpenSettingsPanel()
        {
            pausePanel.SetActive(false);
            settingsPanel.SetActive(true);
        }

        public void CloseSettingsPanel()
        {
            settingsPanel.SetActive(false);
            pausePanel.SetActive(true);
        }

        public void OpenStatsPanel()
        {
            statsPanel.SetActive(true);
            pausePanel.SetActive(false);
        }

        public void CloseStatsPanel()
        {
            statsPanel.SetActive(false);
            pausePanel.SetActive(true);
        }

        private void OpenInventory()
        {
            if (!uiManager.IsUIActive())
            {
                inventoryPanel.SetActive(true);
                pauseButton.SetActive(false);
                uiManager.ActivateUI();
            }
        }

        public void CloseInventory()
        {
            inventoryPanel.SetActive(false);
            pauseButton.SetActive(true);
            uiManager.DeactivateUI();

            if (sellingItemsLogic.PanelIsActive())
            {
                sellingItemsLogic.CloseShop();
            }
        }

        public void CloseUpgradePanel()
        {
            upgradePanel.SetActive(false);
            pauseButton.SetActive(true);
            uiManager.DeactivateUI();
        }

        public void CloseShopPanel()
        {
            shopPanel.SetActive(false);
            pauseButton.SetActive(true);
            uiManager.DeactivateUI();
        }

        public void CloseSellingPanel()
        {
            sellingItemsLogic.CloseShop();
            pauseButton.SetActive(true);
            uiManager.DeactivateUI();
        }

        public void SwitchOffPauseButton()
        {
            pauseButton.SetActive(false);
        }

        public void OpenMainMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}