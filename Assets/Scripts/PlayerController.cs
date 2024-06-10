using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator; // Reference to the Animator component
    private bool isMining = false; // Flag to track if the player is mining
    private bool isPatrolModeActive = false; // Flag to track if patrol mode is active

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        // Set the "IsMining" parameter in the animator
        animator.SetBool("IsMining", isMining);

        // Check if the player is moving
        if (navMeshAgent.velocity.magnitude > 0)
        {
            // Set the parameter in the Animator to play the "RunForward" animation
            animator.SetBool("IsRunning", true);
        }
        else
        {
            // If not moving, play the "Idle" animation
            animator.SetBool("IsRunning", false);
        }
    }

    public void SetPatrolMode(bool isActive)
    {
        // Enable or disable patrolling mode
        isPatrolModeActive = isActive;
        if (!isActive)
        {
            navMeshAgent.ResetPath(); // Stop any ongoing patrol
        }
    }

    public void SetPatrolDestination(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
        isPatrolModeActive = true;
    }

    public void MoveTowardsMineableRock(GameObject rock)
    {
        navMeshAgent.SetDestination(rock.transform.position);
        isMining = true;
    }

    public void ToggleMining()
    {
        isMining = !isMining;
        // Stop mining animation
        if (!isMining)
        {
            animator.SetBool("IsMining", false);
        }
    }

    // Method to find the nearest mineable rock
    public void MoveTowardsMineableRock()
    {
        // Find the nearest mineable rock and move towards it
        GameObject[] mineableRocks = GameObject.FindGameObjectsWithTag("MineableRock");
        if (mineableRocks.Length > 0)
        {
            GameObject nearestRock = FindNearestObject(mineableRocks);
            if (nearestRock != null)
            {
                navMeshAgent.SetDestination(nearestRock.transform.position);
                isMining = true;
            }
        }
    }

    private GameObject FindNearestObject(GameObject[] objects)
    {
        GameObject nearestObject = null;
        float nearestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(obj.transform.position, currentPosition);
            if (distance < nearestDistance)
            {
                nearestObject = obj;
                nearestDistance = distance;
            }
        }
        return nearestObject;
    }
}
