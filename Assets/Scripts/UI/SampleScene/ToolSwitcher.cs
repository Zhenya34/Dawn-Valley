using UnityEngine;

public class ToolSwitcher : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _toolIcon;
    [SerializeField] private Player_Animation _playerAnim;
    [SerializeField] private Sprite[] _toolSprites;

    private ToolType _currentTool = ToolType.Hand;

    private void Awake()
    {
        _playerAnim.UpdateToolType(ToolType.Hand);
    }

    public enum ToolType
    {
        Pickaxe = 0,
        Axe = 1,
        WateringCan = 2,
        Hoe = 3,
        Sword = 4,
        Hand = 5
    }

    private void Update()
    {
        ChangeToolType();
    }

    private void ChangeToolType()
    {
        float scroll = Input.GetAxis("MouseScrollWheel");
        if (scroll != 0)
        {
            int newToolIndex = (int)_currentTool + (scroll > 0 ? 1 : -1);

            if (newToolIndex >= System.Enum.GetValues(typeof(ToolType)).Length)
            {
                newToolIndex = 0;
            }
            else if (newToolIndex < 0)
            {
                newToolIndex = System.Enum.GetValues(typeof(ToolType)).Length - 1;
            }

            _currentTool = (ToolType)newToolIndex;

            _playerAnim.UpdateToolType(_currentTool);
            UpdateToolIcon();
        }
    }

    private void UpdateToolIcon()
    {
        int toolIndex = (int)_currentTool;
        if (toolIndex >= 0 && toolIndex < _toolSprites.Length)
        {
            _toolIcon.sprite = _toolSprites[toolIndex];
        }
    }

    public ToolType GetCurrentTool()
    {
        return _currentTool;
    }
}

