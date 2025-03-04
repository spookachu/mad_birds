using UnityEngine;

// <summary>
/// Moves an targets in a specified direction (up/down or left/right)
/// to increase level difficulty
//  </summary>
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
