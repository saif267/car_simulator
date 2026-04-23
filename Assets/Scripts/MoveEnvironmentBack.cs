using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnvironmentBack : MonoBehaviour
{
    public GameObject[] groundTiles;    // Assign green ground pieces in Inspector
    public float startSpeed = 3f;       // Starting speed
    public float maxSpeed = 15f;        // Max speed
    public float acceleration = 0.3f;   // Speed increase rate

    private float tileLength;
    private float currentSpeed;

    void Start()
    {
        currentSpeed = startSpeed;

        // Auto-detect tile length (Z direction)
        if (groundTiles.Length > 0)
        {
            Renderer rend = groundTiles[0].GetComponentInChildren<Renderer>();
            tileLength = rend.bounds.size.z;
        }
    }

    void Update()
    {
        //  Speed up over time
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);

        for (int i = 0; i < groundTiles.Length; i++)
        {
            // Move ground backward
            groundTiles[i].transform.Translate(Vector3.back * currentSpeed * Time.deltaTime, Space.World);

            // If tile goes behind player, recycle it
            if (groundTiles[i].transform.position.z < -tileLength)
            {
                // Find farthest tile in front
                float maxZ = float.MinValue;
                foreach (GameObject t in groundTiles)
                {
                    if (t.transform.position.z > maxZ)
                        maxZ = t.transform.position.z;
                }

                // Move current tile in front of farthest
                groundTiles[i].transform.position = new Vector3(
                    groundTiles[i].transform.position.x,
                    groundTiles[i].transform.position.y,
                    maxZ + tileLength
                );
            }
        }
    }
}
