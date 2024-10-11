using System;
using UnityEngine;

public class PlantsGrowth : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private int _daysToNextStage;
    [SerializeField] private GameObject _harvestedObject;
    [SerializeField] private Sprite[] _plantsStages = new Sprite[4];
    [SerializeField] private float _clickRadius = 0.3f;

    private int _currentStage = 0;
    private bool _canBeCollected = false;
    private readonly int _maxStage = 3;
    private int _daysSincePlanted = 0;
    private Vector3Int _cellPosition;
    private ToolSwitcher _toolswitcher;
    private Planting _plantingSystem;

    static public bool isWithinHarvestingReach = false;

    private void Awake()
    {
        if (_plantsStages.Length > 0)
        {
            _sr.sprite = _plantsStages[_currentStage];
        }

        try
        {
            _toolswitcher = GameObject.FindGameObjectWithTag(Tags.TollIconUI.ToString()).GetComponent<ToolSwitcher>();
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

        _cellPosition = _plantingSystem.GetCellPosition(transform.position);
    }

    private enum Tags
    {
        Player,
        TollIconUI
    }

    private void Start()
    {
        DayNightCycle dayNightCycle = FindObjectOfType<DayNightCycle>();
        if (dayNightCycle != null)
        {
            dayNightCycle.AddPlant(this);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _toolswitcher.GetCurrentTool() == ToolSwitcher.ToolType.Hoe)
        {
            Vector2 plantPosition = transform.position;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float distanceToPlant = Vector2.Distance(mousePos, plantPosition);

            if (distanceToPlant <= _clickRadius && isWithinHarvestingReach)
            {
                HarvestPlants();
            }
        }
    }

    private void HarvestPlants()
    {
        if (_canBeCollected && _harvestedObject != null)
        {
            Instantiate(_harvestedObject, transform.position, Quaternion.identity);
            _plantingSystem.FreeCell(_cellPosition);
            Destroy(gameObject);
        }
    }

    public void CheckGrowthProgress()
    {
        _daysSincePlanted++;

        if (_daysSincePlanted >= _daysToNextStage && _currentStage < _maxStage)
        {
            _currentStage++;
            _sr.sprite = _plantsStages[_currentStage];
            _daysSincePlanted = 0;

            if (_currentStage == _maxStage)
            {
                _canBeCollected = true;
            }
        }
    }
}