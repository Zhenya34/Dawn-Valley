using System.Collections.Generic;
using Animals.Pets.Bee;
using Animals.Pets.Ghost;
using Animals.Pets.globalAnimControllers;
using Animals.Pets.Namespace;
using UnityEngine;
using UnityEngine.AI;

namespace Animals.Pets.Movement
{
    public class PetsMovementController : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float followRadius;
        [SerializeField] private float avoidRadius;
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private GlobalPetAnimController globalPetAnimController;
        [SerializeField] private CrawlingPetAnimController crawlingPetAnimController;
        [SerializeField] private GhostPetAnimController ghostPetAnimController;
        [SerializeField] private BeePetAnimController beePetAnimController;

        private bool _isNightTime = false;

        private void Awake()
        {
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
        }

        private void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > followRadius)
            {
                MoveTowardsPlayer();
            }
            else if (distanceToPlayer < avoidRadius)
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
            navMeshAgent.SetDestination(player.position);
            navMeshAgent.isStopped = false;
            SetRunningAnimation();
        }

        private void MoveAwayFromPlayer()
        {
            Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
            Vector3 avoidPosition = transform.position + directionAwayFromPlayer * followRadius;

            if (NavMesh.SamplePosition(avoidPosition, out NavMeshHit hit, followRadius, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position);
                navMeshAgent.isStopped = false;
                SetRunningAnimation();
            }
        }

        private void StopMovement()
        {
            navMeshAgent.isStopped = true;
            UpdateNightTimeForControllers();
        }

        private void SetRunningAnimation()
        {
            globalPetAnimController.SetRunningAnimation();
            UpdateNightTimeForControllers();
        }

        private void UpdateNightTimeForControllers()
        {
            List<INightTimeController> nightTimeControllers = new()
            {
                beePetAnimController,
                ghostPetAnimController,
                crawlingPetAnimController,
                globalPetAnimController
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
            if (navMeshAgent.velocity.magnitude > 0.1f)
            {
                bool isMovingRight = navMeshAgent.velocity.x > 0;
                sr.flipX = !isMovingRight;
            }
        }
    }
}