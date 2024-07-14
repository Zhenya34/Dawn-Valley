using UnityEngine;
using UnityEngine.UI;

public class PlayerCoinsWallet : MonoBehaviour
{
    private int _playerCoins;

    [SerializeField] private Text _coinsText;

    private void Start()
    {
        UpdatePlayerCoinsUI();
    }

    public bool SpendCoins(int amount)
    {
        if (_playerCoins >= amount)
        {
            _playerCoins -= amount;
            UpdatePlayerCoinsUI();
            return true;
        }
        return false;
    }

    public void AddCoins(int amount)
    {
        _playerCoins += amount;
        UpdatePlayerCoinsUI();
    }

    private void UpdatePlayerCoinsUI()
    {
        if (_coinsText != null)
        {
            _coinsText.text = "Coins: " + _playerCoins.ToString();
        }
    }


    public void DeductCoins(int amount)
    {
        _playerCoins -= amount;
        UpdatePlayerCoinsUI();
    }

    public bool CanAfford(int amount)
    {
        return _playerCoins >= amount;
    }
}