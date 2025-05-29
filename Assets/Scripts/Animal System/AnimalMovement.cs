using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AnimalMovement : MonoBehaviour
{
    [SerializeField] private float cooldownTime;
    [SerializeField] private float wanderRadius = 5f;
    [SerializeField] private bool useSpawnPointAsCenter = true;

    private NavMeshAgent agent;
    private float cooldownTimer;
    private Vector3 centerPoint;

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
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;

            Vector3 wanderCenter = useSpawnPointAsCenter ? centerPoint : transform.position;
            randomDirection += wanderCenter;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas);

            Vector3 targetPos = hit.position;
            agent.SetDestination(targetPos);
            cooldownTimer = cooldownTime;
        }
    }
}