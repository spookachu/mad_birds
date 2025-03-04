using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f; 
    public float gravity = -9.8f; 
    public float fallSpeed = 0f;

    private CharacterController controller;
    private Vector3 moveDirection;
    private bool canMove = true;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Handles camera movement and rotation based on user input.
    /// </summary>
    void Update()
    {
        if (canMove)
        {
            HandleMovement();
            HandleRotation();
        }
    }

    /// <summary>
    /// Moves the camera based on player input (WASD or Arrow Keys).
    /// </summary>
    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.forward * vertical + transform.right * horizontal;
        if (controller.isGrounded){
            fallSpeed = 0f;  
        }
        else{
            fallSpeed += gravity * Time.deltaTime; 
        }
        move.y = fallSpeed;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Rotates the camera left or right based on arrow key input.
    /// </summary>
    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }

    public void EnableMovement(bool enable)
    {
        canMove = enable;
    }
}