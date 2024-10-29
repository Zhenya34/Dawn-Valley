using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light2D _globalLight2D;
    [SerializeField] private float _dayDurationInMinutes;
    [SerializeField] private List<PetsMovementController> _petsMovementControllers;
    [SerializeField] private WateringCanLogic _wateringCanLogic;

    private int _days = 1;
    private float _currentTimeOfDay = 0.5f;
    private float _timeMultiplier;
    private bool _wasNightTime = false;
    private readonly List<PlantsGrowth> _plantsGrowth = new();

    private void Start()
    {
        _timeMultiplier = 1.0f / (_dayDurationInMinutes * 60.0f);
    }

    private void Update()
    {
        _currentTimeOfDay += Time.deltaTime * _timeMultiplier;
        _currentTimeOfDay %= 1;

        UpdateLighting();

        bool isNightTime = _currentTimeOfDay < 0.5f;

        if (isNightTime != _wasNightTime)
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
                IncrementDay();
                _wateringCanLogic.CheckForDryingPlots();
                UpdatePlantGrowth();
            }

            _wasNightTime = isNightTime;
        }
    }

    private void UpdateLighting()
    {
        if (_globalLight2D != null)
        {
            _globalLight2D.intensity = Mathf.Lerp(0.1f, 1f, Mathf.Clamp01(1 - Mathf.Abs(_currentTimeOfDay - 0.5f) * 2));
            _globalLight2D.color = Color.Lerp(Color.blue, Color.white, Mathf.Clamp01(1 - Mathf.Abs(_currentTimeOfDay - 0.5f) * 2));
        }
    }

    private void UpdatePlantGrowth()
    {
        for (int i = _plantsGrowth.Count - 1; i >= 0; i--)
        {
            if (_plantsGrowth[i] != null)
            {
                _plantsGrowth[i].CheckGrowthProgress();
            }
            else
            {
                _plantsGrowth.RemoveAt(i);
            }
        }
    }

    public void AddPlant(PlantsGrowth plant)
    {
        _plantsGrowth.Add(plant);
    }

    public int GetCurrentDay()
    {
        return _days;
    }

    private void IncrementDay()
    {
        _days++;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
