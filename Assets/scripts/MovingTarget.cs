using UnityEngine;

/// <summary>
/// Moves an object back and forth in a specified direction.
//  The movement speed and distance can be freely adjusted.
/// </summary>
public class MovingTarget : MonoBehaviour
{
    public Vector3 movementDirection = new Vector3(0, 1, 0); 
    public float speed = 2f;
    public float distance = 2f; 

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float movement = Mathf.Sin(Time.time * speed) * distance;
        transform.position = startPosition + movementDirection * movement;
    }
}
