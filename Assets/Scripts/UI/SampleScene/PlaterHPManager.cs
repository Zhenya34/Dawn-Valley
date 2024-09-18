using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPManager : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite[] _sprites = new Sprite[7];
    [SerializeField] private float _triggerRadius;
    [SerializeField] private GameObject _textObject;

    private int _currentPlayerHealth;
    private const int _maxPlayerHealth = 100;
    private const int _minPlayerHealth = 0;
    private TextMeshProUGUI _textComponent;


    private void Start()
    {
        _currentPlayerHealth = _maxPlayerHealth;
        UpdateHealthSprite();
    }

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        float distance = Vector3.Distance(mousePosition, transform.position);

        if (distance <= _triggerRadius)
        {
            _textObject.SetActive(true);
        }
        else
        {
            _textObject.SetActive(false);
        }
    }

    public void UpdatePlayerHealth(int damage)
    {
        _currentPlayerHealth -= damage;
        _currentPlayerHealth = Mathf.Clamp(_currentPlayerHealth, _minPlayerHealth, _maxPlayerHealth);
        _textObject.TryGetComponent(out _textComponent);
        _textComponent.text = _currentPlayerHealth.ToString() + "%";

        UpdateHealthSprite();
    }

    private void UpdateHealthSprite()
    {
        int spriteIndex = Mathf.FloorToInt((_currentPlayerHealth / (float)_maxPlayerHealth) * (_sprites.Length - 1));
        spriteIndex = Mathf.Clamp(spriteIndex, 0, _sprites.Length - 1);
        _image.sprite = _sprites[spriteIndex];
    }
}