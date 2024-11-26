using Player.ToolsLogic;
using UnityEngine;

namespace Player.EventDispatchers
{
    public class EventDispatcher : MonoBehaviour
    {
        [SerializeField] private WateringCanLogic wateringCanLogic;
        [SerializeField] private HoeLogic hoeLogic;
    
        public void DispatchWateringEvent()
        {
            wateringCanLogic.HandleWatering();
        }

        public void DispatchHoeLogicEvent()
        {
            hoeLogic.HandleHoeLogic();
        }
    }
}
