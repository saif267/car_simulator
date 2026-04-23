using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float verticalInput;
    
     public float horizontalInput;

    public float forwardInput;

  

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        verticalInput = Input.GetAxis("Vertical");     // W/S or Up/Down
    horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right

    // Move forward at a constant rate
    transform.Translate(Vector3.forward * Time.deltaTime * speed);

    // Pitch (tilt up/down) around the local X-axis
    transform.Rotate(Vector3.right, Time.deltaTime * rotationSpeed * verticalInput);

    // (Optional) Yaw (turn left/right) around the local Y-axis
    transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed * horizontalInput);
        
         
    }
}
