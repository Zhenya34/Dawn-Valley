using UnityEngine;
using UnityEngine.AI;

public class PetsMovementController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _followRadius;
    [SerializeField] private float _avoidRadius;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private GlobalPetAnimController _globalPetAnimController;
    [SerializeField] private CrawlingPetAnimController _crawlingPetAnimController;
    [SerializeField] private GhostPetAnimController _ghostPetAnimController;
    [SerializeField] private BeePetAnimController _beePetAnimController;

    private bool _isNightTime = false;

    private void Awake()
    {
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (distanceToPlayer > _followRadius)
        {
            _navMeshAgent.SetDestination(_player.position);
            _navMeshAgent.isStopped = false;

            if (_globalPetAnimController != null)
            {
                _globalPetAnimController.SetRunningAnimation();
            }

            if(_beePetAnimController != null && _isNightTime == true)
            {
                _beePetAnimController.DeactivateNightTime();
            }

            if(_ghostPetAnimController != null && _isNightTime == true)
            {
                _ghostPetAnimController.DeactivateNightTime();
            }

            if(_crawlingPetAnimController != null && _isNightTime == true)
            {
                _crawlingPetAnimController.DeactivateNightTime();
            }
        }
        else if (distanceToPlayer < _avoidRadius)
        {
            Vector3 directionAwayFromPlayer = (transform.position - _player.position).normalized;
            Vector3 avoidPosition = transform.position + directionAwayFromPlayer * _followRadius;

            if (NavMesh.SamplePosition(avoidPosition, out NavMeshHit hit, _followRadius, NavMesh.AllAreas))
            {
                _navMeshAgent.SetDestination(hit.position);
                _navMeshAgent.isStopped = false;

                if (_globalPetAnimController != null)
                {
                    _globalPetAnimController.SetRunningAnimation();
                }

                if (_beePetAnimController != null && _isNightTime == true)
                {
                    _beePetAnimController.DeactivateNightTime();
                }

                if (_ghostPetAnimController != null && _isNightTime == true)
                {
                    _ghostPetAnimController.DeactivateNightTime();
                }

                if (_crawlingPetAnimController != null && _isNightTime == true)
                {
                    _crawlingPetAnimController.DeactivateNightTime();
                }
            }
        }
        else
        {
            _navMeshAgent.isStopped = true;
            if (_globalPetAnimController != null)
            {
                if (_isNightTime)
                {
                    _globalPetAnimController.ActivateNightTime();
                }
                else
                {
                    _globalPetAnimController.DeactivateNightTime();
                }
            }
            if (_beePetAnimController != null)
            {
                if (_isNightTime)
                {
                    _beePetAnimController.ActivateNightTime();
                }
                else
                {
                    _beePetAnimController.DeactivateNightTime();
                }
            }
            if (_ghostPetAnimController != null)
            {
                if (_isNightTime)
                {
                    _ghostPetAnimController.ActivateNightTime();
                }
                else
                {
                    _ghostPetAnimController.DeactivateNightTime();
                }
            }
            if (_crawlingPetAnimController != null)
            {
                if (_isNightTime)
                {
                    _crawlingPetAnimController.ActivateNightTime();
                }
                else
                {
                    _crawlingPetAnimController.DeactivateNightTime();
                }
            }
        }
        UpdateSpriteDirection();
    }

    public void ActivateNightTime()
    {
        _isNightTime = true;
    }

    public void DeactivateNightTime()
    {
        _isNightTime = false;
    }

    private void UpdateSpriteDirection()
    {
        if (_navMeshAgent.velocity.magnitude > 0.1f)
        {
            bool isMovingRight = _navMeshAgent.velocity.x > 0;
            _sr.flipX = !isMovingRight;
        }
    }
}