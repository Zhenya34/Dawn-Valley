using System.Collections.Generic;
using Animals.Pets.Movement;
using Enviroment.Plants;
using Player.ToolsLogic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Enviroment.Time
{
    public class DayNightCycle : MonoBehaviour
    {
        [SerializeField] private Light2D globalLight2D;
        [SerializeField] private float dayDurationInMinutes;
        [SerializeField] private List<PetsMovementController> petsMovementControllers;
        [SerializeField] private WateringCanLogic wateringCanLogic;

        private int _days = 1;
        private float _currentTimeOfDay = 0.5f;
        private float _timeMultiplier;
        private bool _wasNightTime = false;
        private readonly List<PlantsGrowth> _plantsGrowth = new();

        private void Start()
        {
            _timeMultiplier = 1.0f / (dayDurationInMinutes * 60.0f);
        }

        private void Update()
        {
            _currentTimeOfDay += UnityEngine.Time.deltaTime * _timeMultiplier;
            _currentTimeOfDay %= 1;

            UpdateLighting();

            bool isNightTime = _currentTimeOfDay < 0.5f;

            if (isNightTime != _wasNightTime)
            {
                if (isNightTime)
                {
                    foreach (var petController in petsMovementControllers)
                    {
                        petController.ActivateNightTime();
                    }
                }
                else
                {
                    foreach (var petController in petsMovementControllers)
                    {
                        petController.DeactivateNightTime();
                    }
                    IncrementDay();
                    wateringCanLogic.CheckForDryingPlots();
                    UpdatePlantGrowth();
                }

                _wasNightTime = isNightTime;
            }
        }

        private void UpdateLighting()
        {
            if (globalLight2D != null)
            {
                globalLight2D.intensity = Mathf.Lerp(0.1f, 1f, Mathf.Clamp01(1 - Mathf.Abs(_currentTimeOfDay - 0.5f) * 2));
                globalLight2D.color = Color.Lerp(Color.blue, Color.white, Mathf.Clamp01(1 - Mathf.Abs(_currentTimeOfDay - 0.5f) * 2));
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
            UnityEngine.Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            UnityEngine.Time.timeScale = 1;
        }
    }
}
