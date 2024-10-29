using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : MonoBehaviour
{
    [SerializeField] private WateringCanLogic _wateringCanLogic;
    [SerializeField] private HoeLogic _hoeLogic;
    
    public void DispatchWateringEvent()
    {
        _wateringCanLogic.HandleWatering();
    }

    public void DispatchHoeLogicEvent()
    {
        _hoeLogic.HandleHoeLogic();
    }
}
