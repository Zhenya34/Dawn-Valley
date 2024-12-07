using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SampleScene.Shop
{
    [System.Serializable]
    public class Upgrade
    {
        public string name;
        public Button upgradeButton;
        public int maxLevel;
        public int currentLevel;
        public int[] costPerLevel;
    }

    public class UpgradeManager : MonoBehaviour
    {
        [SerializeField] private Upgrade[] upgrades;
        [SerializeField] private PlayerCoinsWallet playerCoinsWallet;
        [SerializeField] private SampleSceneCanvasLogic sampleSceneCanvasLogic;
        [SerializeField] private GameObject player;
        [SerializeField] private float shopRadius;
        [SerializeField] private GameObject upgradePanel;
        [SerializeField] private UIManager.UIManager uiManager;

        private void Start() => InitializeShop();

        private void OnMouseDown()
        {
            if (!uiManager.IsUIActive())
            {
                float distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance <= shopRadius)
                {
                    upgradePanel.SetActive(true);
                    uiManager.ActivateUI();
                    sampleSceneCanvasLogic.SwitchOffPauseButton();
                }
            }
        }

        private void InitializeShop()
        {
            foreach (var upgrade in upgrades)
            {
                UpdateUpgradeButton(upgrade);
                upgrade.upgradeButton.onClick.AddListener(() => OnUpgradeButtonClicked(upgrade));
            }
        }

        private void OnUpgradeButtonClicked(Upgrade upgrade)
        {
            if (upgrade.currentLevel < upgrade.maxLevel)
            {
                int cost = upgrade.costPerLevel[upgrade.currentLevel];
                if (playerCoinsWallet.SpendCoins(cost))
                {
                    upgrade.currentLevel++;
                    UpdateUpgradeButton(upgrade);
                }
            }
        }

        private void UpdateUpgradeButton(Upgrade upgrade)
        {
            if (upgrade.currentLevel >= upgrade.maxLevel)
            {
                upgrade.upgradeButton.interactable = false;
                upgrade.upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Max Level";
            }
            else
            {
                int cost = upgrade.costPerLevel[upgrade.currentLevel];
                upgrade.upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = cost.ToString();
            }
        }
    }
}