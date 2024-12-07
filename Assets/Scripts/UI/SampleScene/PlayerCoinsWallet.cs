using TMPro;
using UnityEngine;

namespace UI.SampleScene
{
    public class PlayerCoinsWallet : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinsText;

        private int _playerCoins;  

        private void Start() => UpdatePlayerCoinsUI();

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
            if (coinsText)
            {
                coinsText.text = _playerCoins.ToString();
            }
        }

        public void DeductCoins(int amount)
        {
            _playerCoins -= amount;
            UpdatePlayerCoinsUI();
        }

        public bool CanAfford(int amount) => _playerCoins >= amount;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                AddCoins(100);
            }
        }
    }
}