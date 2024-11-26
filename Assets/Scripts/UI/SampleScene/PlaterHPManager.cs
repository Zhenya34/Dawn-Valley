using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SampleScene
{
    public class PlayerHpManager : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite[] sprites = new Sprite[7];
        [SerializeField] private float triggerRadius;
        [SerializeField] private GameObject textObject;

        private int _currentPlayerHealth;
        private const int MaxPlayerHealth = 100;
        private const int MinPlayerHealth = 0;
        private TextMeshProUGUI _textComponent;


        private void Start()
        {
            _currentPlayerHealth = MaxPlayerHealth;
            UpdateHealthSprite();
        }

        private void Update()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            float distance = Vector3.Distance(mousePosition, transform.position);

            if (distance <= triggerRadius)
            {
                textObject.SetActive(true);
            }
            else
            {
                textObject.SetActive(false);
            }
        }

        public void UpdatePlayerHealth(int damage)
        {
            _currentPlayerHealth -= damage;
            _currentPlayerHealth = Mathf.Clamp(_currentPlayerHealth, MinPlayerHealth, MaxPlayerHealth);
            textObject.TryGetComponent(out _textComponent);
            _textComponent.text = _currentPlayerHealth.ToString() + "%";

            UpdateHealthSprite();
        }

        private void UpdateHealthSprite()
        {
            int spriteIndex = Mathf.FloorToInt((_currentPlayerHealth / (float)MaxPlayerHealth) * (sprites.Length - 1));
            spriteIndex = Mathf.Clamp(spriteIndex, 0, sprites.Length - 1);
            image.sprite = sprites[spriteIndex];
        }
    }
}