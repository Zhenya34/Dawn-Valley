using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Upgrade[] _upgrades;
    [SerializeField] private PlayerCoinsWallet _playerCoinsWallet;
    [SerializeField] private SampleSceneCanvasLogic _sampleSceneCanvasLogic;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _shopRadius;
    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private UIManager _uiManager;

    private void Start()
    {
        InitializeShop();
    }

    private void OnMouseDown()
    {
        if (!_uiManager.IsUIActive())
        {
            float distance = Vector3.Distance(_player.transform.position, transform.position);
            if (distance <= _shopRadius)
            {
                _upgradePanel.SetActive(true);
                _uiManager.ActivateUI();
                _sampleSceneCanvasLogic.SwitchOffPauseButton();
            }
        }
    }

    private void InitializeShop()
    {
        foreach (var upgrade in _upgrades)
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
            if (_playerCoinsWallet.SpendCoins(cost))
            {
                upgrade.currentLevel++;
                ApplyUpgradeEffects(upgrade);
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

    private void ApplyUpgradeEffects(Upgrade upgrade)
    {
        Debug.Log("ApplyEffects");
    }
}