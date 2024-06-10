using UnityEngine;
using UnityEngine.AI;

public class PatrolBehavior : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 initialPosition;
    private Vector3 patrolPoint;
    private bool patrolPointSet = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        initialPosition = transform.position;
    }

    public void SetPatrolPoint(Vector3 point)
    {
        patrolPoint = point;
        patrolPointSet = true;
        GoToPatrolPoint();
    }

    void GoToPatrolPoint()
    {
        if (patrolPointSet)
        {
            agent.destination = patrolPoint;
        }
    }

    void Update()
    {
        if (patrolPointSet && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Vector3 nextDestination = (agent.destination == patrolPoint) ? initialPosition : patrolPoint;
            agent.destination = nextDestination;
        }
    }
}
