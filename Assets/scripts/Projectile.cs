using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for projectile behavior 
//  Holds shared logic for different game types
/// </summary>
public class Projectile : MonoBehaviour
{
    public Rigidbody rb;
    public bool isThrown = false;
    public float launchForce = 10f;    
    public Vector3 startPoint;
    public Vector3 originalPosition;
    public Quaternion originalRotation;

    // Trajectory
    public float maxDragDistance = 3f;
    public bool isDragging = false;
    public Vector3 dragDirection;
    public LineRenderer trajectoryLine;
    public int trajectoryResolution = 30;

    //Effects
     public GameObject puffEffect; 

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public virtual void Update()
    {
    }

    public virtual void OnMouseDown()
    {
        startPoint = transform.position;
        isDragging = true;
        isThrown = false;
    }

    public virtual void OnMouseDrag()
    {
        Vector3 dragPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5f));
        dragDirection = Vector3.ClampMagnitude(dragPosition - startPoint, maxDragDistance);
        transform.position = startPoint;
        DrawTrajectory(transform.position, dragDirection * launchForce);
    }

    public virtual void OnMouseUp()
    {
        if (!isThrown)
        {
            isThrown = true;
            isDragging = false;
            rb.isKinematic = false;
            Launch(dragDirection * launchForce);
            trajectoryLine.positionCount = 0;;
        }
    }

    /// <summary>
    /// Launches the projectile with a given velocity.
    /// </summary>
    public void Launch(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    /// <summary>
    /// Draws the predicted trajectory of the projectile using physics equations.
    /// s = s0 + v0*t + 1/2*a*t^2
    /// </summary>
    public void DrawTrajectory(Vector3 startPos, Vector3 startVelocity)
    {
        trajectoryLine.positionCount = trajectoryResolution;
        Vector3[] points = new Vector3[trajectoryResolution];

        for (int i = 0; i < trajectoryResolution; i++)
        {
            float time = i * 0.05f;
            points[i] = startPos + startVelocity * time + 0.5f * Physics.gravity * time * time;
        }
        trajectoryLine.SetPositions(points);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
    }

    /// <summary>
    /// Resets the projectile to its original position.
    /// </summary>
    public virtual void ResetProjectile()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        isThrown = false;

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    /// <summary>
    /// Triggers the puff effect and resets the projectile.
    /// </summary>
    public IEnumerator DisappearWithPuff()
    {
        if (puffEffect != null)
        {
            GameObject puff = Instantiate(puffEffect, transform.position, Quaternion.identity);
            ParticleSystem puffParticleSystem = puff.GetComponent<ParticleSystem>();
            if (puffParticleSystem != null)
            {
                var mainModule = puffParticleSystem.main;
                mainModule.loop = false;
                mainModule.startSize = 0.5f;
                mainModule.startLifetime = 1f;
                mainModule.startSpeed = 0.5f;
                puffParticleSystem.Play();
                yield return new WaitForSeconds(1.5f);
                Destroy(puff);
            }
        }
        ResetProjectile();
    }

    public bool RestartGame()
    {
        if (Input.GetKeyDown(KeyCode.R))
            {
                ResetProjectile(); 
                return true;
            }
        return false;
    }
}
