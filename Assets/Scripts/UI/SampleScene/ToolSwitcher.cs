using Player;
using Player.Placement;
using UnityEngine;

namespace UI.SampleScene
{
    public class ToolSwitcher : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Image toolIcon;
        [SerializeField] private PlayerAnimation playerAnim;
        [SerializeField] private Sprite[] toolSprites;
        [SerializeField] private PlacementSystem placementSystem;

        private ToolType _currentTool = ToolType.Hand;
        private bool _isToolChangeAvaliable;

        private void Awake() => playerAnim.UpdateToolType(ToolType.Hand);

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
            if (_isToolChangeAvaliable)
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

                    playerAnim.UpdateToolType(_currentTool);
                    UpdateToolIcon();
                    UpdatePlacementMode();
                }
            }
        }

        private void UpdatePlacementMode()
        {
            if (ToolType.Pickaxe == _currentTool)
            {
                placementSystem.StartRemoving();
            }
            else
            {
                placementSystem.StopRemoving();
            }
        }

        private void UpdateToolIcon()
        {
            int toolIndex = (int)_currentTool;
            if (toolIndex >= 0 && toolIndex < toolSprites.Length)
            {
                toolIcon.sprite = toolSprites[toolIndex];
            }
        }

        public ToolType GetCurrentTool() => _currentTool;
        
        public void AllowToolChanges() => _isToolChangeAvaliable = true;

        public void ProhibitToolChanges() => _isToolChangeAvaliable = false;
    }
}

