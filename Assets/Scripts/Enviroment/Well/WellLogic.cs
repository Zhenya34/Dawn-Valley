using Player.ToolsLogic;
using UI.SampleScene;
using UnityEngine;

namespace Enviroment.Well
{
    public class WellLogic : MonoBehaviour
    {
        [SerializeField] private ToolSwitcher toolSwitcher;
        [SerializeField] private WateringCanLogic wateringCanLogic;

        private void OnMouseDown()
        {
            if (toolSwitcher.GetCurrentTool() == ToolSwitcher.ToolType.WateringCan) wateringCanLogic.RefillWateringCan();
        }
    }
}