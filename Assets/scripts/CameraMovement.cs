using UnityEngine;

// predecessor to PlayerMovement
public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float rotationSpeed = 2f;

    /// <summary>
    /// Handles camera movement and rotation based on user input.
    /// </summary>
    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    /// <summary>
    /// Moves the camera based on player input (WASD or Arrow Keys).
    /// </summary>
    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical"); 

        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward = forward.normalized;

        Vector3 right = transform.right;
        right.y = 0f;
        right = right.normalized;

        Vector3 moveDirection = forward * vertical + right * horizontal;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// Rotates the camera left or right based on arrow key input.
    /// </summary>
    void HandleRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow)){
            transform.Rotate(Vector3.up, -rotationSpeed);
        }
        if (Input.GetKey(KeyCode.RightArrow)){
            transform.Rotate(Vector3.up, rotationSpeed);
        }
    }
}
