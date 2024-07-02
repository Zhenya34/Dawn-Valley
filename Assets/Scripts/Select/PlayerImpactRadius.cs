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
            _tileSelector.canPlaceFrame = true;
        }
        else
        {
            _tileSelector.canPlaceFrame = false;
        }

        if (distanceToTools <= _toolsRadius)
        {
            _playerAnim.toolsAllowed = true;
            PlantsGrowth.isWithinHarvestingReach = true;
        }
        else
        {
            _playerAnim.toolsAllowed = false;
            PlantsGrowth.isWithinHarvestingReach = false;
        }
    }
}