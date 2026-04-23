using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadDestroyer : MonoBehaviour
{
    public GameObject roadPrefab;   // The road prefab to spawn
    public Transform spawnPoint;    // Position where new roads will spawn (ahead of the last road)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("road"))
        {
            // Save position of the road being destroyed
            Vector3 oldPos = other.transform.position;

            // Destroy the old road tile
            Destroy(other.gameObject);

            // Spawn a new road tile ahead
            Vector3 newPos = new Vector3(oldPos.x, oldPos.y, spawnPoint.position.z);
            Instantiate(roadPrefab, newPos, Quaternion.identity);
        }
    }
}
