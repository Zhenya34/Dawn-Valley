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
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
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

            var triggerPosition = transform.position;
            var distance = Vector3.Distance(mousePosition, triggerPosition);
            var distanceToTools = Vector3.Distance(mousePosition, triggerPosition);

            if (distance <= triggerFrameRadius)
                tileSelector.AllowFramePlacement();
            else
                tileSelector.ProhibitFramePlacement();

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

        public float GetToolDistanceValue() => toolsRadius;
    }
}