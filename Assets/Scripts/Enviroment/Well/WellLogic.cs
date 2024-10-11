using UnityEngine;

public class WellLogic : MonoBehaviour
{
    [SerializeField] private ToolSwitcher _toolSwitcher;
    [SerializeField] private WateringCanLogic _wateringCanLogic;

    private void OnMouseDown()
    {
        if (_toolSwitcher.GetCurrentTool() == ToolSwitcher.ToolType.WateringCan)
        {
            _wateringCanLogic.RefillWateringCan();
        }
    }
}