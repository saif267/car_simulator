using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessRoad : MonoBehaviour
{
    public Transform player;      // Player reference
    public float roadLength = 50f; // Length of one road segment
    public int numRoads = 3;      // Total road segments in the scene

    private float offset;

    void Start()
    {
        // Precalculate offset (distance to move road forward when recycling)
        offset = roadLength * numRoads;
    }

    void Update()
    {
        // If this road is far behind the player → move it forward
        if (transform.position.z + roadLength < player.position.z)
        {
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + offset);
            transform.position = newPos;
        }
    }
}
