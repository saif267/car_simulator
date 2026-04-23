using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositeCar : MonoBehaviour
{
    public float speed = 10f;
    public float respawnDistance = 200f; 
    public Transform player;

    private float laneX;
    private bool isRespawning = false; // prevents multiple coroutines

    void Start()
    {
        laneX = transform.position.x; // lock lane at start
    }

    void Update()
    {
        // Keep moving the car
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);

        // If car has already passed the player and is not yet queued for respawn
        if (!isRespawning && transform.position.z < player.position.z - 5f)
        {
            StartCoroutine(RespawnWithDelay());
        }
    }

    IEnumerator RespawnWithDelay()
    {
        isRespawning = true;

        // Let it fully disappear (optional wait before removing)
        yield return new WaitForSeconds(1.5f); //delay (1–2 seconds)

        float newZ = player.position.z + respawnDistance;

        // Respawn in the same lane
        transform.position = new Vector3(laneX, transform.position.y, newZ);

        isRespawning = false;
    }
}


