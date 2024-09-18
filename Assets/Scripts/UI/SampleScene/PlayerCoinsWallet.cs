using TMPro;
using UnityEngine;

public class PlayerCoinsWallet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;

    private int _playerCoins;  

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
            _coinsText.text = _playerCoins.ToString();
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))//удали потом 
        {
            AddCoins(100);
        }
    }
}