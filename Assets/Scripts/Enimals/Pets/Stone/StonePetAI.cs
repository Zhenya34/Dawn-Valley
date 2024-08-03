using UnityEngine;
using UnityEngine.AI;
using DawnValley.Utils;

public class StonePetAI : MonoBehaviour
{
    [SerializeField] private State _startingState;
    [SerializeField] private float _walkingDistanceMax;
    [SerializeField] private float _walkingDistanceMin;
    [SerializeField] private float _walkingTimerMax;

    private NavMeshAgent _navMeshAgent;
    private State _currentState;
    private float _walkingTime;
    private Vector3 _walkPosition;
    private Vector3 _startingPosition;

    private enum State
    {
        Idle,
        Walking
    }

    private void Start()
    {
        _startingPosition = transform.position;
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _currentState = _startingState;
    }

    private void Update()
    {
        if (_currentState == State.Idle)
        {
            // Ничего не делаем в состоянии Idle
        }
        else if (_currentState == State.Walking)
        {
            _walkingTime -= Time.deltaTime;
            if (_walkingTime < 0)
            {
                Walking();
                _walkingTime = _walkingTimerMax;
            }
        }
    }

    private void Walking()
    {
        _walkPosition = GetWalkingPosition();
        _navMeshAgent.SetDestination(_walkPosition);
    }

    private Vector3 GetWalkingPosition()
    {
        return _startingPosition + Utils.GetRandomDir() * Random.Range(_walkingDistanceMin, _walkingDistanceMax);
    }
}
