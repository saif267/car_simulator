using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPatch : MonoBehaviour
{
    public float speed = 10f;          // Speed road moves backward
    public float roadLength = 50f;     // Length of one patch
    public GameObject roadPrefab;      // The road prefab to spawn
    public static Transform lastRoad;  // Keeps track of the last road’s position

    void Start()
    {
        // When game starts, set this patch as last road if none exists
        if (lastRoad == null)
            lastRoad = transform;
    }

    void Update()
    {
        // Move road backward
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroyer")) // Plane behind the player
        {
            // Spawn new road at the end of the last one
            Vector3 spawnPos = lastRoad.position + Vector3.forward * roadLength;
            GameObject newRoad = Instantiate(roadPrefab, spawnPos, Quaternion.identity);

            // Update lastRoad reference
            lastRoad = newRoad.transform;

            // Destroy this road
            Destroy(gameObject);
        }
    }
}



