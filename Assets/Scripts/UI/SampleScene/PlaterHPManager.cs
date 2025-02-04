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
        private Camera _camera;

        private void Awake() => _camera = Camera.main;

        private void Start()
        {
            _currentPlayerHealth = MaxPlayerHealth;
            UpdateHealthSprite();
        }

        private void Update()
        {
            if (!_camera) return;
            if (!Application.isFocused) return;
                
            var mouseScreenPos = Input.mousePosition;
                
            if (mouseScreenPos.x < 0 || mouseScreenPos.x > Screen.width ||
                mouseScreenPos.y < 0 || mouseScreenPos.y > Screen.height)
            {
                return;
            }
                
            var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            var distance = Vector3.Distance(mousePosition, transform.position);
            textObject.SetActive(distance <= triggerRadius);
        }

        public void UpdatePlayerHealth(int damage)
        {
            _currentPlayerHealth -= damage;
            _currentPlayerHealth = Mathf.Clamp(_currentPlayerHealth, MinPlayerHealth, MaxPlayerHealth);
            textObject.TryGetComponent(out _textComponent);
            _textComponent.text = _currentPlayerHealth + "%";

            UpdateHealthSprite();
        }

        private void UpdateHealthSprite()
        {
            var spriteIndex = Mathf.FloorToInt((_currentPlayerHealth / (float)MaxPlayerHealth) * (sprites.Length - 1));
            spriteIndex = Mathf.Clamp(spriteIndex, 0, sprites.Length - 1);
            image.sprite = sprites[spriteIndex];
        }
    }
}