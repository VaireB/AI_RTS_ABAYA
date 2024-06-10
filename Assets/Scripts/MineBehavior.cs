using UnityEngine;
using UnityEngine.AI;

public class MineBehavior : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the MiningAgent.");
            return;
        }

        GameObject[] mineableRocks = GameObject.FindGameObjectsWithTag("MineableRock");
        if (mineableRocks.Length > 0)
        {
            GameObject nearestRock = FindNearestRock(mineableRocks);
            if (nearestRock != null)
            {
                agent.SetDestination(nearestRock.transform.position);
            }
            else
            {
                Debug.LogError("No mineable rock found.");
            }
        }
        else
        {
            Debug.LogError("No mineable rock objects found in the scene.");
        }
    }

    GameObject FindNearestRock(GameObject[] rocks)
    {
        GameObject nearestRock = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject rock in rocks)
        {
            float distance = Vector3.Distance(transform.position, rock.transform.position);
            if (distance < nearestDistance)
            {
                nearestRock = rock;
                nearestDistance = distance;
            }
        }

        return nearestRock;
    }

    void Update()
    {
        if (agent != null && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // Simulate mining
            Debug.Log("Mining...");
        }
    }
}
