using UnityEngine;
using UnityEngine.SceneManagement; 
using CameraFading;

public class MinigameBoundary : MonoBehaviour
{
    public GameObject camera;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    private FirstPersonLook cameraMovementScript;
    private GameObject boundary;
    public GameObject anchor;
    private Vector3 anchorPosition;
    private Quaternion anchorRotation;
    private bool isInMinigame = false;

    // player settings
    private GameObject player;
    private Rigidbody playerRigidbody;
    private PlayerMovement playerMovement;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Start()
    {
        originalCameraPosition = Camera.main.transform.position;
        originalCameraRotation = Camera.main.transform.rotation;
        cameraMovementScript = camera.GetComponent<FirstPersonLook>();
        anchorPosition = anchor.transform.position;
        anchorRotation = anchor.transform.rotation;

        player = GameObject.FindGameObjectWithTag("Player");
        playerRigidbody = player.GetComponent<Rigidbody>();
        Transform meshTransform = player.transform.Find("Capsule Mesh");
        playerMovement = player.GetComponent<PlayerMovement>();
        originalPosition = meshTransform.transform.position;
        originalRotation = meshTransform.transform.rotation;
    }

    /// <summary>
    /// Allows to quit minigame and reset when pressing Q
    /// </summary>
    private void Update()
    {
        if (isInMinigame && Input.GetKeyDown(KeyCode.Q)){
            EndMinigame();
    }
        // Restart the scene to reset everything
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // This method triggers when the player's collider enters the boundary collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log("Player entered the boundary, starting mini-game.");
            CameraFade.Out();
            StartMinigame();
        }
    }

    private void StartMinigame()
    {
        isInMinigame = true;
        camera.transform.position = anchor.transform.position;
        camera.transform.rotation = anchor.transform.rotation;

        playerMovement.EnableMovement(false);
        cameraMovementScript.enabled = false;
        CameraFade.In();
    }

    private void EndMinigame()
    {
        isInMinigame = false;
        
        player.transform.position = originalPosition;
        player.transform.rotation = originalRotation;

        ResetCamera();
        cameraMovementScript.enabled = true;
        playerMovement.EnableMovement(true);
    }

    private void ResetCamera()
    {
        camera.transform.position = player.transform.position + (originalCameraPosition - originalPosition);
        camera.transform.rotation = originalCameraRotation;
    }
}
