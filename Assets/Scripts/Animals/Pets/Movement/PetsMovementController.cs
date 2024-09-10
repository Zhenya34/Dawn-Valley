using UnityEngine.AI;
using UnityEngine;
using System.Collections.Generic;
using AnimControllerNamespace;

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
            MoveTowardsPlayer();
        }
        else if (distanceToPlayer < _avoidRadius)
        {
            MoveAwayFromPlayer();
        }
        else
        {
            StopMovement();
        }

        UpdateSpriteDirection();
    }

    private void MoveTowardsPlayer()
    {
        _navMeshAgent.SetDestination(_player.position);
        _navMeshAgent.isStopped = false;
        SetRunningAnimation();
    }

    private void MoveAwayFromPlayer()
    {
        Vector3 directionAwayFromPlayer = (transform.position - _player.position).normalized;
        Vector3 avoidPosition = transform.position + directionAwayFromPlayer * _followRadius;

        if (NavMesh.SamplePosition(avoidPosition, out NavMeshHit hit, _followRadius, NavMesh.AllAreas))
        {
            _navMeshAgent.SetDestination(hit.position);
            _navMeshAgent.isStopped = false;
            SetRunningAnimation();
        }
    }

    private void StopMovement()
    {
        _navMeshAgent.isStopped = true;
        UpdateNightTimeForControllers();
    }

    private void SetRunningAnimation()
    {
        _globalPetAnimController.SetRunningAnimation();
        UpdateNightTimeForControllers();
    }

    private void UpdateNightTimeForControllers()
    {
        List<INightTimeController> nightTimeControllers = new()
        {
            _beePetAnimController,
            _ghostPetAnimController,
            _crawlingPetAnimController,
            _globalPetAnimController
        };

        foreach (var controller in nightTimeControllers)
        {
            if (controller != null)
            {
                if (_isNightTime)
                {
                    controller.ActivateNightTime();
                }
                else
                {
                    controller.DeactivateNightTime();
                }
            }
        }
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