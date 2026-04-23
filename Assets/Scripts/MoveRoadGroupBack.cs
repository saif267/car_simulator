using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoadGroupBack : MonoBehaviour
{
    public GameObject[] roadTiles;  //  3 road tiles
    public float startSpeed = 5f;   // Starting speed
    public float maxSpeed = 40f;    // Max speed limit
    public float acceleration = 0.5f; // How quickly speed increases

    private float tileLength;
    private float currentSpeed;

    void Start()
    {
        currentSpeed = startSpeed;

        // Detect tile length automatically
        if (roadTiles.Length > 0)
        {
            Renderer rend = roadTiles[0].GetComponentInChildren<Renderer>();
            tileLength = rend.bounds.size.z;
        }
    }

    void Update()
    {
        //  Gradually increase speed
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);

        for (int i = 0; i < roadTiles.Length; i++)
        {
            // Move the road backwards
            roadTiles[i].transform.Translate(Vector3.back * currentSpeed * Time.deltaTime, Space.World);

            // When a tile goes behind the player, recycle it
            if (roadTiles[i].transform.position.z < -tileLength)
            {
                // Find farthest tile ahead
                float maxZ = float.MinValue;
                foreach (GameObject t in roadTiles)
                {
                    if (t.transform.position.z > maxZ)
                        maxZ = t.transform.position.z;
                }

                // Move this tile in front of last one
                roadTiles[i].transform.position = new Vector3(
                    roadTiles[i].transform.position.x,
                    roadTiles[i].transform.position.y,
                    maxZ + tileLength
                );
            }
        }
    }
}
