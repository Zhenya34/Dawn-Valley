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

    private void Start()
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
            upgrade.upgradeButton.GetComponentInChildren<Text>().text = "Max Level";
        }
        else
        {
            int cost = upgrade.costPerLevel[upgrade.currentLevel];
            upgrade.upgradeButton.GetComponentInChildren<Text>().text = "Upgrade (" + cost + " coins)";
        }
    }

    private void ApplyUpgradeEffects(Upgrade upgrade)
    {
        switch (upgrade.name)
        {
            case "House":
                UpgradeHouse(upgrade.currentLevel);
                break;
                // Добавьте сюда другие типы прокачек и их эффекты
                // case "OtherUpgrade":
                //     ApplyOtherUpgradeEffects(upgrade.currentLevel);
                //     break;
        }
    }

    private void UpgradeHouse(int level)
    {
        // Пример смены сцены в зависимости от уровня дома
        string sceneName = "HouseLevel" + level.ToString();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}