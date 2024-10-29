using UnityEngine;

public class PlayerImpactRadius : MonoBehaviour
{
    [SerializeField] private float _triggerFrameRadius = 2f;
    [SerializeField] private float _toolsRadius = 1.5f;
    [SerializeField] private TileSelector _tileSelector;
    [SerializeField] private Player_Animation _playerAnim;

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 triggerPosition = transform.position;
        float distance = Vector3.Distance(mousePosition, triggerPosition);
        float distanceToTools = Vector3.Distance(mousePosition, triggerPosition);

        if (distance <= _triggerFrameRadius)
        {
            _tileSelector.AllowFramePlacement();
        }
        else
        {
            _tileSelector.ProhibitFramePlacement();
        }

        if (distanceToTools <= _toolsRadius)
        {
            _playerAnim.AllowToolsUsing();
            PlantsGrowth.isWithinHarvestingReach = true;
        }
        else
        {
            _playerAnim.ProhibitToolsUsing();
            PlantsGrowth.isWithinHarvestingReach = false;
        }
    }

    public float GetToolDistanceValue()
    {
        return _toolsRadius;
    }
}