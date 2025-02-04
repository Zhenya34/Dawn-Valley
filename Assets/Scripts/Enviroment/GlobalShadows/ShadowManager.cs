using System;
using System.Collections.Generic;
using Enviroment.Time;
using UnityEngine;
using Player;

namespace Enviroment.GlobalShadows
{
    public class ShadowManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private DayNightCycle dayNightCycle;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private float updateInterval = 2f;

        private readonly List<ShadowController> _visibleShadows = new();
        private float _lastUpdateTime;
        private Vector3 _lastPlayerPosition;

        private const float MaxShadowAlpha = 132f;
        private const float MinShadowAlpha = 0f;

        private void Start()
        {
            if (!mainCamera) mainCamera = Camera.main;
            _lastPlayerPosition = playerMovement.transform.position;
        }

        private void Update()
        {
            if (UnityEngine.Time.time - _lastUpdateTime >= updateInterval || HasPlayerMoved())
            {
                UpdateVisibleObjects();
                _lastUpdateTime = UnityEngine.Time.time;
                _lastPlayerPosition = playerMovement.transform.position;
            }

            UpdateShadows();
        }

        private bool HasPlayerMoved()
        {
            return Vector3.Distance(_lastPlayerPosition, playerMovement.transform.position) > 0.1f;
        }

        private void UpdateVisibleObjects()
        {
            _visibleShadows.Clear();
            
            var allShadows = FindObjectsOfType<ShadowController>();

            foreach (var shadow in allShadows)
            {
                if (IsVisibleFromCamera(shadow.GetComponent<Renderer>()))
                {
                    _visibleShadows.Add(shadow);
                }
            }
        }

        private bool IsVisibleFromCamera(Renderer render)
        {
            if (!render) return false;

            var planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
            return GeometryUtility.TestPlanesAABB(planes, render.bounds);
        }

        private void UpdateShadows()
        {
            var shadowAlpha = Mathf.Lerp(MinShadowAlpha, MaxShadowAlpha, dayNightCycle.GetCurrentShadowIntensity());

            foreach (var shadow in _visibleShadows)
            {
                shadow.SetShadowAlpha(Convert.ToInt32(shadowAlpha));
            }
        }
    }
}