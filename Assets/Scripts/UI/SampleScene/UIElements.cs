using UnityEngine;

namespace UI.SampleScene
{
    public class UIElements
    {
        public GameObject PausePanel { get; private set; }
        public GameObject SettingsPanel { get; private set; }
        public GameObject StatsPanel { get; private set; }
        public GameObject PauseButton { get; private set; }
        public GameObject InventoryPanel { get; private set; }
        public GameObject UpgradePanel { get; private set; }
        public GameObject ShopPanel { get; private set; }

        public UIElements(GameObject pausePanel, GameObject settingsPanel, GameObject statsPanel, 
            GameObject pauseButton, GameObject inventoryPanel, 
            GameObject upgradePanel, GameObject shopPanel)
        {
            PausePanel = pausePanel;
            SettingsPanel = settingsPanel;
            StatsPanel = statsPanel;
            PauseButton = pauseButton;
            InventoryPanel = inventoryPanel;
            UpgradePanel = upgradePanel;
            ShopPanel = shopPanel;
        }
    }
}