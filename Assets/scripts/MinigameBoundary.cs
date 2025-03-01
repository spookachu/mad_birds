using UnityEngine;

public class MinigameBoundary : MonoBehaviour
{
    public Vector3 originalCameraPosition;
    public Quaternion originalCameraRotation;

    private CameraMovement cameraMovementScript;
    private GameObject player;
    private GameObject boundary;
    public GameObject anchor;
    private Vector3 anchorPosition;
    private Quaternion anchorRotation;

    private void Start()
    {
        originalCameraPosition = Camera.main.transform.position;
        originalCameraRotation = Camera.main.transform.rotation;

        cameraMovementScript = Camera.main.GetComponent<CameraMovement>();
        player = GameObject.FindGameObjectWithTag("Player");

        anchorPosition = anchor.transform.position;
        anchorRotation = anchor.transform.rotation;
    }

    private void Update()
    {
    }

    // This method triggers when the player's collider enters the boundary collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log("Player entered the boundary, starting mini-game.");
            StartMinigame();
        }
    }

    private void StartMinigame()
    {
        Camera.main.transform.position = anchorPosition;
        Camera.main.transform.rotation = anchorRotation;

        if (cameraMovementScript != null){
            cameraMovementScript.enabled = false;
        }
    }

    private void ResetCamera()
    {
        Camera.main.transform.position = originalCameraPosition;
        Camera.main.transform.rotation = originalCameraRotation;
    }
}
