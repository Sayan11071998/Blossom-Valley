using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AnimalMovement : MonoBehaviour
{
    [SerializeField] private float cooldownTime;
    [SerializeField] private float wanderRadius = 5f;

    private NavMeshAgent agent;
    private float cooldownTimer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cooldownTimer = Random.Range(0, cooldownTime);
    }

    private void Update() => Wander();

    public void ToggleMovement(bool enabled) => agent.enabled = enabled;

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
            randomDirection += transform.position;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas);

            Vector3 targetPos = hit.position;
            agent.SetDestination(targetPos);
            cooldownTimer = cooldownTime;
        }
    }
}