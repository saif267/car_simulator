using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // For restarting the game

public class CarCollision : MonoBehaviour
{
    public int maxCollisions = 3;   // Allowed collisions
    private int collisionCount = 0;

    public GameObject explosionEffect; // Optional explosion effect prefab

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Count this enemy as one hit only
            collisionCount++;
            Debug.Log("Collision Count: " + collisionCount);

            // Destroy the enemy vehicle so it doesn't keep hitting
            Destroy(collision.gameObject);

            // Check if player has no lives left
            if (collisionCount >= maxCollisions)
            {
                // Optional: explosion effect on player
                if (explosionEffect != null)
                {
                    Instantiate(explosionEffect, transform.position, Quaternion.identity);
                }

                Destroy(gameObject); // Destroy player car
                Invoke("RestartGame", 2f); // Restart scene after 2 seconds
            }
        }
    }

    void RestartGame()
    {
        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

