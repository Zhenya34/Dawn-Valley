using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private List<PetsMovementController> _petsMovementControllers;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            foreach (var petController in _petsMovementControllers)
            {
                petController.ActivateNightTime();
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            foreach (var petController in _petsMovementControllers)
            {
                petController.DeactivateNightTime();
            }
        }
    }
}
