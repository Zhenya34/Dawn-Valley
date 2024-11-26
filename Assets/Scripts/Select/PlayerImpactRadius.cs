using Enviroment.Plants;
using Player;
using UnityEngine;

namespace Select
{
    public class PlayerImpactRadius : MonoBehaviour
    {
        [SerializeField] private float triggerFrameRadius;
        [SerializeField] private float toolsRadius;
        [SerializeField] private TileSelector tileSelector;
        [SerializeField] private PlayerAnimation playerAnim;

        private void Update()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            Vector3 triggerPosition = transform.position;
            float distance = Vector3.Distance(mousePosition, triggerPosition);
            float distanceToTools = Vector3.Distance(mousePosition, triggerPosition);

            if (distance <= triggerFrameRadius)
            {
                tileSelector.AllowFramePlacement();
            }
            else
            {
                tileSelector.ProhibitFramePlacement();
            }

            if (distanceToTools <= toolsRadius)
            {
                playerAnim.AllowToolsUsing();
                PlantsGrowth.IsWithinHarvestingReach = true;
            }
            else
            {
                playerAnim.ProhibitToolsUsing();
                PlantsGrowth.IsWithinHarvestingReach = false;
            }
        }

        public float GetToolDistanceValue()
        {
            return toolsRadius;
        }
    }
}