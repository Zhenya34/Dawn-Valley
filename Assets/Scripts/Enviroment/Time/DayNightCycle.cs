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
        [SerializeField] private Color nightColor;

        private int _days = 1;
        private float _currentTimeOfDay = 0.5f;
        private float _timeMultiplier;
        private bool _wasNightTime;
        private readonly List<PlantsGrowth> _plantsGrowth = new();
        private readonly List<Light2D> _lamps = new();

        private void Start() => _timeMultiplier = 1.0f / (dayDurationInMinutes * 60.0f);

        private void Update()
        {
            _currentTimeOfDay += UnityEngine.Time.deltaTime * _timeMultiplier;
            _currentTimeOfDay %= 1;

            UpdateLighting();
        }

        private void UpdateLighting()
        {
            if (globalLight2D)
            {
                float intensity = Mathf.Lerp(0.1f, 1f, Mathf.Clamp01(1 - Mathf.Abs(_currentTimeOfDay - 0.5f) * 2));
                globalLight2D.intensity = intensity;
                globalLight2D.color = Color.Lerp(nightColor, Color.white, Mathf.Clamp01(1 - Mathf.Abs(_currentTimeOfDay - 0.5f) * 2));
                
                if (intensity <= 0.25f && !_wasNightTime)
                {
                    EnterNight();
                    _wasNightTime = true;
                }
                else if (intensity > 0.4f && _wasNightTime)
                {
                    EnterDay();
                    _wasNightTime = false;
                }
            }
        }

        private void EnterNight()
        {
            ActivateLamps(true);
            foreach (var petController in petsMovementControllers)
            {
                if (!petController.gameObject.activeInHierarchy)
                    continue;
                
                petController.ActivateNightTime();
                petController.UpdateNightTimeForControllers();
            }
        }

        private void EnterDay()
        {
            ActivateLamps(false);

            foreach (var petController in petsMovementControllers)
            {
                if (petController || !petController.gameObject.activeInHierarchy)
                    continue;
                
                petController.DeactivateNightTime();
            }

            IncrementDay();
            wateringCanLogic.CheckForDryingPlots();
            UpdatePlantGrowth();
        }

        private void ActivateLamps(bool activate)
        {
            foreach (var lamp in _lamps) lamp.enabled = activate;
        }

        public void AddLamp(Light2D lamp)
        {
            if (lamp && !_lamps.Contains(lamp)) _lamps.Add(lamp);
        }

        public void RemoveLamp(Light2D lamp)
        {
            if (lamp && _lamps.Contains(lamp)) _lamps.Remove(lamp);
        }

        private void UpdatePlantGrowth()
        {
            for (int i = _plantsGrowth.Count - 1; i >= 0; i--)
            {
                if (_plantsGrowth[i])
                {
                    _plantsGrowth[i].CheckGrowthProgress();
                }
                else
                {
                    _plantsGrowth.RemoveAt(i);
                }
            }
        }
        
        public float GetCurrentShadowIntensity()
        {
            return Mathf.Clamp01(1 - Mathf.Abs(_currentTimeOfDay - 0.5f) * 2);
        }

        public void AddPlant(PlantsGrowth plant) => _plantsGrowth.Add(plant);

        public int GetCurrentDay() => _days;

        private void IncrementDay() => _days++;

        public void PauseGame() => UnityEngine.Time.timeScale = 0;

        public void ResumeGame() => UnityEngine.Time.timeScale = 1;
    }
}