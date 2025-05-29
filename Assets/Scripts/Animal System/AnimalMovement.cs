using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AnimalMovement : MonoBehaviour
{
    NavMeshAgent agent; 

    //Time before the navmesh sets another destination to move towards
    [SerializeField] float cooldownTime;
    float cooldownTimer; 
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cooldownTimer = Random.Range(0, cooldownTime);
    }

    public void ToggleMovement(bool enabled)
    {
        agent.enabled = enabled; 
    }

    // Update is called once per frame
    void Update()
    {
        Wander(); 
    }

    void Wander()
    {
        if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            //Generate a random direction within a sphere with a radius of 10
            Vector3 randomDirection = Random.insideUnitSphere * 10f;

            //Offset the random direction by the current position of the animal
            randomDirection += transform.position;

            NavMeshHit hit;
            //Sample the nearest valid position on the Navmesh
            NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);

            //Get the final target position
            Vector3 targetPos = hit.position;

            agent.SetDestination(targetPos);
            cooldownTimer = cooldownTime; 
        }
        

    }
}
