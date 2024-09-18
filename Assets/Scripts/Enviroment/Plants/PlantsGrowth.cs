using System;
using System.Collections;
using UnityEngine;

public class PlantsGrowth : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private int _daysToNextStage;
    [SerializeField] private GameObject _harvestedObject;
    [SerializeField] private Sprite[] _plantsStages = new Sprite[4];
    [SerializeField] private Player_Animation _playerAnim;
    [SerializeField] private float _clickRadius = 0.3f;

    private int _currentStage = 0;
    private bool _canBeCollected = false;
    private readonly int _maxStage = 3;

    static public bool isWithinHarvestingReach = false;

    private void Awake()
    {
        if (_plantsStages.Length > 0)
        {
            _sr.sprite = _plantsStages[_currentStage];
            StartCoroutine(GrowthPlants());
        }

        try
        {
            _playerAnim = GameObject.FindGameObjectWithTag(Tags.Player.ToString()).GetComponent<Player_Animation>();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    private enum Tags
    {
        Player
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _playerAnim.GetToolTypeValue() == 3)
        {
            Vector2 plantPosition = transform.position;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float distanceToPlant = Vector2.Distance(mousePos, plantPosition);

            if (distanceToPlant <= _clickRadius && isWithinHarvestingReach == true)
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
            Destroy(gameObject);
        }
    }

    private IEnumerator GrowthPlants()
    {
        while (_currentStage < _maxStage)
        {
            yield return new WaitForSeconds(_daysToNextStage);
            _currentStage++;
            _sr.sprite = _plantsStages[_currentStage];

            if (_currentStage == _maxStage)
            {
                _canBeCollected = true;
                yield break;
            }
        }
    }
}