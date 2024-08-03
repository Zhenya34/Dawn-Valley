using UnityEngine;
using UnityEngine.AI;

public class PetsMovementController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _followRadius;
    [SerializeField] private float _avoidRadius;

    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        GetReference();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
    }

    private void GetReference()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if(distanceToPlayer > _followRadius)
        {
            Debug.Log(">_followRadius");
            _navMeshAgent.SetDestination(_player.position);
            _navMeshAgent.isStopped = false;
        }
        else if(distanceToPlayer < _avoidRadius)
        {
            Debug.Log("<_avoidRadius");
            Vector3 directionAwayFromPlayer = (transform.position - _player.position).normalized;
            Vector3 avoidPosition = transform.position + directionAwayFromPlayer * _avoidRadius;
            _navMeshAgent.SetDestination(avoidPosition);
            _navMeshAgent.isStopped = false;
        }               
        else
        {
            _navMeshAgent.isStopped = true;
        }
    }
}
