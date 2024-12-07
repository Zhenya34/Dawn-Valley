using Enviroment.Other;
using UnityEngine;
using UnityEngine.AI;

namespace Animals.Animals
{
    public class WalkingAnimalsAI : MonoBehaviour
    {
        [SerializeField] private State startingState;
        [SerializeField] private float walkingDistanceMax;
        [SerializeField] private float walkingDistanceMin;
        [SerializeField] private float walkingTimerMax;

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

        private void Start() => _startingPosition = transform.position;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.updateUpAxis = false;
            _currentState = startingState;
        }

        private void Update()
        {
            if (_currentState == State.Walking)
            {
                _walkingTime -= Time.deltaTime;
                if (_walkingTime < 0)
                {
                    Walking();
                    _walkingTime = walkingTimerMax;
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
            return _startingPosition + Utils.GetRandomDir() * Random.Range(walkingDistanceMin, walkingDistanceMax);
        }
    }
}