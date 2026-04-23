using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadObject : MonoBehaviour
{
    public float speed;                   // Movement speed towards player
    public float baseRespawnDistance = 100f;   // Default respawn distance
    public Transform player;

    [Header("Spawn Boundaries (X Axis)")]
    public float leftBoundary = -4f;       // Left edge of the road
    public float rightBoundary = 4f;       // Right edge of the road

    [Header("Spacing Settings")]
    public float minSpacingX = 2f;         // Minimum horizontal gap
    public float minSpacingZ = 5f;         // Minimum forward/backward gap

    // Keep track of active car positions
    private static List<Vector3> activeCars = new List<Vector3>();

    private float respawnDistance;         // This will change per difficulty


    void Start()
    {
        // Set values based on difficulty
        switch (GameManager.currentDifficulty)
        {
            case GameManager.Difficulty.Easy:
                speed = 25f;
                respawnDistance = baseRespawnDistance * 1.2f; // cars spawn further away (slower spawn rate)
                break;
            case GameManager.Difficulty.Medium:
                speed = 50f;
                respawnDistance = baseRespawnDistance; // normal spawn
                break;
            case GameManager.Difficulty.Hard:
                speed = 80f;
                respawnDistance = baseRespawnDistance * 0.7f; // cars spawn closer (faster spawn rate)
                break;
        }
    }

    void OnEnable()
    {
        // Register when spawned
        activeCars.Add(transform.position);
    }

    void OnDisable()
    {
        // Remove when destroyed 
        activeCars.Remove(transform.position);
    }

    void Update()
    {
        // Move vehicle backwards (towards player)
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);

        // Check if vehicle passed behind player
        if (transform.position.z < player.position.z - 10f)
        {
            // Try respawning at a valid new position
            Vector3 newPos = GetValidRespawnPosition();
            transform.position = newPos;
        }

        // Keep list updated
        for (int i = 0; i < activeCars.Count; i++)
        {
            if (activeCars[i] == transform.position)
            {
                activeCars[i] = transform.position;
            }
        }
    }

    Vector3 GetValidRespawnPosition()
    {
        float newZ = player.position.z + respawnDistance;
        Vector3 newPos;

        int attempts = 0;
        bool valid;

        do
        {
            // Pick random X within boundary
            float randomX = Random.Range(leftBoundary, rightBoundary);
            newPos = new Vector3(randomX, transform.position.y, newZ);

            // Check against all active cars
            valid = true;
            foreach (Vector3 carPos in activeCars)
            {
                float distX = Mathf.Abs(newPos.x - carPos.x);
                float distZ = Mathf.Abs(newPos.z - carPos.z);

                if (distX < minSpacingX && distZ < minSpacingZ)
                {
                    valid = false;
                    break;
                }
            }

            attempts++;
            if (attempts > 20)
            {
                break; // Fail-safe in case space is too full
            }
        } while (!valid);

        return newPos;
    }

    //  Collision with Player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            Time.timeScale = 0f;

            StartCoroutine(ShowGameOverUI());

            FindObjectOfType<GameManager>().GameOver();

            Debug.Log("Game Over: Player car destroyed!");
        }
    }

    private IEnumerator ShowGameOverUI()
{
        // Wait 2 seconds in real time
        yield return new WaitForSecondsRealtime(2f);

        FindObjectOfType<GameManager>().GameOver();
}
}
