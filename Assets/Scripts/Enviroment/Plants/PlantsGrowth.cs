using System;
using Enviroment.Time;
using Player.ToolsLogic;
using UI.SampleScene;
using UnityEngine;

namespace Enviroment.Plants
{
    public class PlantsGrowth : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private int daysToNextStage;
        [SerializeField] private GameObject harvestedObject;
        [SerializeField] private Sprite[] plantsStages = new Sprite[4];
        [SerializeField] private float clickRadius = 0.3f;

        private int _currentStage;
        private bool _canBeCollected;
        private readonly int _maxStage = 3;
        private int _daysSincePlanted;
        private Vector3Int _cellPosition;
        private ToolSwitcher _toolSwitcher;
        private Planting _plantingSystem;
        private WateringCanLogic _wateringCanLogic;

        static public bool IsWithinHarvestingReach = false;

        private void Awake()
        {
            if (plantsStages.Length > 0)
            {
                sr.sprite = plantsStages[_currentStage];
            }

            try
            {
                _toolSwitcher = GameObject.FindGameObjectWithTag(Tags.TollIconUI.ToString()).GetComponent<ToolSwitcher>();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }

            try
            {
                _plantingSystem = GameObject.FindGameObjectWithTag(Tags.Player.ToString()).GetComponent<Planting>();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }

            try
            {
                _wateringCanLogic = GameObject.FindGameObjectWithTag(Tags.Player.ToString()).GetComponent<WateringCanLogic>();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }

            if (_plantingSystem)
            {
                _cellPosition = _plantingSystem.GetCellPosition(transform.position);
                _plantingSystem.RegisterPlant(_cellPosition, this);
            }
        }

        private enum Tags
        {
            Player,
            TollIconUI
        }

        private void Start()
        {
            DayNightCycle dayNightCycle = FindObjectOfType<DayNightCycle>();
            if (dayNightCycle)
            {
                dayNightCycle.AddPlant(this);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _toolSwitcher.GetCurrentTool() == ToolSwitcher.ToolType.Hoe)
            {
                Vector2 plantPosition = transform.position;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float distanceToPlant = Vector2.Distance(mousePos, plantPosition);

                if (distanceToPlant <= clickRadius && IsWithinHarvestingReach)
                {
                    HarvestPlants();
                }
            }
        }

        private void HarvestPlants()
        {
            if (_canBeCollected && harvestedObject)
            {
                Instantiate(harvestedObject, transform.position, Quaternion.identity);
                _plantingSystem.FreeCell(_cellPosition);
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (_plantingSystem)
            {
                _plantingSystem.UnregisterPlant(_cellPosition);
            }
        }

        public void CheckGrowthProgress()
        {
            if (_wateringCanLogic.IsGrowingOnWetTile(_cellPosition))
            {
                _daysSincePlanted++;

                if (_daysSincePlanted >= daysToNextStage && _currentStage < _maxStage)
                {
                    _currentStage++;
                    sr.sprite = plantsStages[_currentStage];
                    _daysSincePlanted = 0;

                    if (_currentStage == _maxStage)
                    {
                        _canBeCollected = true;
                    }
                }
            }
        }
    }
}