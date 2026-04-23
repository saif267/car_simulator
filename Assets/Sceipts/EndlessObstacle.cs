using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessObstacle : MonoBehaviour
{
    public Transform player;           // reference to player
    public float respawnDistance = 150f; // how far ahead obstacle reappears
    public float delay = 1f;           // delay after player passes obstacle

    private bool isRespawning = false;
    private float[] lanes = { -2f, 0f, 2f }; // adjust to match your road lanes

    void Update()
    {
        // If the player has passed the obstacle
        if (!isRespawning && transform.position.z < player.position.z - 5f)
        {
            StartCoroutine(RespawnWithDelay());
        }
    }

    IEnumerator RespawnWithDelay()
    {
        isRespawning = true;

        // wait for realism before respawn
        yield return new WaitForSeconds(delay);

        // Choose a new lane (or comment out for same lane)
        float newX = lanes[Random.Range(0, lanes.Length)];
        float newZ = player.position.z + respawnDistance;

        // Respawn ahead
        transform.position = new Vector3(newX, transform.position.y, newZ);

        isRespawning = false;
    }
}
