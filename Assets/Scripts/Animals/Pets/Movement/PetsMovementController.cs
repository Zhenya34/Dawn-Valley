using System.Collections;
using System.Collections.Generic;
using Animals.Pets.Bee;
using Animals.Pets.CrawlingPets;
using Animals.Pets.Ghost;
using Animals.Pets.globalAnimControllers;
using Animals.Pets.Interfaces;
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
        [SerializeField] private float teleportThreshold;
        [SerializeField] private float teleportCheckInterval = 1f;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private PetAnimController petAnimController;
        [SerializeField] private CrawlingPetAnimController crawlingPetAnimController;
        [SerializeField] private GhostPetAnimController ghostPetAnimController;
        [SerializeField] private BeePetAnimController beePetAnimController;

        private bool _isNightTime;
        private bool _isStopped;
        private Coroutine _teleportCoroutine;

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
                StartTeleportCoroutine();
            }
            else if (distanceToPlayer < avoidRadius)
            {
                MoveAwayFromPlayer();
                StopTeleportCoroutine();
            }
            else
            {
                StopMovement();
                StopTeleportCoroutine();
            }

            UpdateSpriteDirection();
            UpdateNightTimeForControllers();
        }

        private void StartTeleportCoroutine()
        {
            if (_teleportCoroutine == null) _teleportCoroutine = StartCoroutine(CheckDistanceToPlayer());
        }

        private void StopTeleportCoroutine()
        {
            if (_teleportCoroutine != null)
            {
                StopCoroutine(_teleportCoroutine);
                _teleportCoroutine = null;
            }
        }

        private IEnumerator CheckDistanceToPlayer()
        {
            while (true)
            {
                yield return new WaitForSeconds(teleportCheckInterval);
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
                if (distanceToPlayer > teleportThreshold) TeleportToPlayer();

                if (distanceToPlayer <= followRadius)
                    yield break;
            }
        }

        private void TeleportToPlayer()
        {
            transform.position = player.position;
        }

        private void MoveTowardsPlayer()
        {
            _isStopped = false;
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
            if (_isStopped) return;
            _isStopped = true;
            navMeshAgent.isStopped = true;

            List<IStateChangeController> stateChangeControllers = new()
            {
                beePetAnimController,
                ghostPetAnimController,
                crawlingPetAnimController
            };

            foreach (var controller in stateChangeControllers)
            {
                controller?.StartChangingStates();
            }
        }

        private void SetRunningAnimation()
        {
            List<IStateChangeController> stateChangeControllers = new()
            {
                beePetAnimController,
                ghostPetAnimController,
                crawlingPetAnimController
            };
            foreach (var controller in stateChangeControllers)
            {
                controller?.StopChangingStates();
            }
            petAnimController?.SetRunningAnimation();
        }

        public void UpdateNightTimeForControllers()
        {
            if (!petAnimController)
                return;

            if (_isNightTime)
            {
                StopMovement();
                petAnimController.ActivateNightTime();
            }
            else
            {
                petAnimController.DeactivateNightTime();
            }
        }

        public void ActivateNightTime() => _isNightTime = true;

        public void DeactivateNightTime() => _isNightTime = false;

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