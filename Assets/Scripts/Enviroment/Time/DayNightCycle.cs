using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light2D globalLight2D;
    [SerializeField] private float dayDurationInMinutes;
    [SerializeField] private List<PetsMovementController> _petsMovementControllers;

    private bool isPaused = false;
    private float currentTimeOfDay = 0.5f;
    private float timeMultiplier;
    private bool wasNightTime = false;

    private void Start()
    {
        timeMultiplier = 1.0f / (dayDurationInMinutes * 60.0f);
    }

    private void Update()
    {
        if (!isPaused)
        {
            currentTimeOfDay += Time.deltaTime * timeMultiplier;
            currentTimeOfDay %= 1;

            UpdateLighting();

            bool isNightTime = currentTimeOfDay < 0.5f;

            if (isNightTime != wasNightTime)
            {
                if (isNightTime)
                {
                    foreach (var petController in _petsMovementControllers)
                    {
                        petController.ActivateNightTime();
                    }
                }
                else
                {
                    foreach (var petController in _petsMovementControllers)
                    {
                        petController.DeactivateNightTime();
                    }
                }

                wasNightTime = isNightTime;
            }
        }
    }

    private void UpdateLighting()
    {
        if (globalLight2D != null)
        {
            globalLight2D.intensity = Mathf.Lerp(0.1f, 1f, Mathf.Clamp01(1 - Mathf.Abs(currentTimeOfDay - 0.5f) * 2));
            globalLight2D.color = Color.Lerp(Color.blue, Color.white, Mathf.Clamp01(1 - Mathf.Abs(currentTimeOfDay - 0.5f) * 2));
        }
    }
}
