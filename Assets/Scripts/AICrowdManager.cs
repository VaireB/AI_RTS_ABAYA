using UnityEngine;
using UnityEngine.AI;

public class CrowdManager : MonoBehaviour
{
    public GameObject aiPrefab; // Prefab of the AI character
    public int crowdSize = 10; // Number of AI characters to spawn
    public float spawnRadius = 3f; // Radius within which AI characters can spawn
    public Transform[] destinations; // Array of destination points for the crowd
    public float minDelay = 1f; // Minimum delay before moving to another destination
    public float maxDelay = 5f; // Maximum delay before moving to another destination

    private NavMeshAgent[] navAgents; // Array to store NavMeshAgent components of AI characters
    private Animator[] animators; // Array to store Animator components of AI characters

    void Start()
    {
        // Spawn the AI crowd
        SpawnAICrowd();

        // Get all NavMeshAgent components attached to AI characters
        navAgents = GetComponentsInChildren<NavMeshAgent>();

        // Get all Animator components attached to AI characters
        animators = GetComponentsInChildren<Animator>();

        // Start coroutine for moving the crowd
        StartCoroutine(MoveCrowd());
    }

    void SpawnAICrowd()
    {
        for (int i = 0; i < crowdSize; i++)
        {
            // Generate a random spawn position within the spawn radius
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;

            // Ensure that the spawn position stays on the ground level
            spawnPosition.y = 0f;

            // Instantiate an AI character at the spawn position
            GameObject aiInstance = Instantiate(aiPrefab, spawnPosition, Quaternion.identity);

            // Parent the AI instance to this crowd manager object for organization
            aiInstance.transform.parent = transform;
        }
    }

    System.Collections.IEnumerator MoveCrowd()
    {
        while (true)
        {
            // Calculate random delay between movements
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            // Get a random destination for the crowd
            Transform randomDestination = destinations[Random.Range(0, destinations.Length)];

            // Move each AI character towards the random destination
            for (int i = 0; i < navAgents.Length; i++)
            {
                // Set random walk animation
                if (animators[i] != null)
                {
                    animators[i].SetBool("IsWalking", true);
                }

                navAgents[i].SetDestination(GetRandomPointInNavMesh(randomDestination.position));
            }
        }
    }

    Vector3 GetRandomPointInNavMesh(Vector3 center)
    {
        Vector3 randomPoint = center + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas);
        return hit.position;
    }
}
