using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AnimalMovement : MonoBehaviour
{
    [SerializeField] private float cooldownTime;

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

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 10f;
            randomDirection += transform.position;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);

            Vector3 targetPos = hit.position;
            agent.SetDestination(targetPos);
            cooldownTimer = cooldownTime;
        }
    }
}