using UnityEngine;
using UnityEngine.AI;

namespace BlossomValley.AnimalSystem
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AnimalMovement : MonoBehaviour
    {
        [SerializeField] private float cooldownTime;
        [SerializeField] private float wanderRadius = 5f;
        [SerializeField] private bool useSpawnPointAsCenter = true;

        private NavMeshAgent agent;
        private float cooldownTimer;
        private Vector3 centerPoint;
        private Bounds roomBounds;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            cooldownTimer = Random.Range(0, cooldownTime);

            if (useSpawnPointAsCenter)
                centerPoint = transform.position;
        }

        private void Update() => Wander();

        public void ToggleMovement(bool enabled) => agent.enabled = enabled;

        public void SetWanderRadius(float newRadius) => wanderRadius = newRadius;

        public void SetWanderCenter(Vector3 newCenter)
        {
            centerPoint = newCenter;
            useSpawnPointAsCenter = true;
        }

        public void SetRoomBounds(Bounds bounds) => roomBounds = bounds;

        private void Wander()
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
                return;
            }

            if (agent == null || !agent.enabled || !agent.isOnNavMesh) return;

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                Vector3 targetPos = GetValidWanderPosition();

                if (targetPos != Vector3.zero)
                {
                    agent.SetDestination(targetPos);
                    cooldownTimer = cooldownTime;
                }
            }
        }

        private Vector3 GetValidWanderPosition()
        {
            int maxAttempts = 10;

            for (int i = 0; i < maxAttempts; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
                Vector3 wanderCenter = useSpawnPointAsCenter ? centerPoint : transform.position;
                randomDirection += wanderCenter;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
                {
                    Vector3 potentialTarget = hit.position;

                    if (roomBounds.size == Vector3.zero || roomBounds.Contains(potentialTarget))
                        return potentialTarget;
                }
            }

            return Vector3.zero;
        }
    }
}