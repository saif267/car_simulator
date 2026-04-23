using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    public float moveSpeed = 10f;   // Left/right movement speed
    public float boundary = 3f;     // Limit so car doesn’t go off the road

    public ParticleSystem explosionparticle;
    private bool hasCollided = false;

    void Update()
    {
        // Get horizontal input (A/D or Left/Right arrow)
        float input = Input.GetAxis("Horizontal"); // returns -1 (left) to +1 (right)

        // Move left/right
        transform.Translate(Vector3.right * input * moveSpeed * Time.deltaTime);

        // Clamp position (keeps car on road)
        float clampedX = Mathf.Clamp(transform.position.x, -boundary, boundary);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation = Quaternion.identity; // resets to default rotation
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !hasCollided)
        {
            hasCollided = true;

            Debug.Log("Player collided with vehicles!");

            // Spawn explosion at player position
            if (explosionparticle != null)
            {
                ParticleSystem explosion = Instantiate(explosionparticle, transform.position, Quaternion.identity);
                explosion.Play();
            }

            // Start Game Over sequence with delays
            StartCoroutine(GameOverSequence());
        }
    }

    private IEnumerator GameOverSequence()
    {
        // Let explosion play for 2 seconds before freezing
        yield return new WaitForSecondsRealtime(2f);

        // Freeze the game
        Time.timeScale = 0f;

        // Trigger Game Over (UI handled by GameManager)
        FindObjectOfType<GameManager>().GameOver();
    }
}
