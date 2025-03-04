using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameBoundary : MonoBehaviour
{
    public Vector3 originalCameraPosition;
    public Quaternion originalCameraRotation;
    public Vector3 originalPlayerPosition;
    public Quaternion originalPlayerRotation;

    private CameraMovement cameraMovementScript;
    private FirstPersonLook cameraRotationScript;
    private PlayerMovement playerMovementScript;
    private GameObject player;
    private GameObject playerParent;
    private GameObject boundary;
    public GameObject anchor;
    private Vector3 anchorPosition;
    private Quaternion anchorRotation;
    private bool isInMinigame = false;

    private void Start()
    {
        originalCameraPosition = Camera.main.transform.position;
        originalCameraRotation = Camera.main.transform.rotation;

        cameraMovementScript = Camera.main.GetComponent<CameraMovement>();
        cameraRotationScript = Camera.main.GetComponent<FirstPersonLook>();
        playerParent = GameObject.FindGameObjectWithTag("Player");
        playerMovementScript = playerParent.GetComponent<PlayerMovement>();
        player = GameObject.FindGameObjectWithTag("PlayerSkin");
        
        anchorPosition = anchor.transform.position;
        anchorRotation = anchor.transform.rotation;

        originalPlayerPosition = player.transform.position;
        originalPlayerRotation = player.transform.rotation;
    }

    /// <summary>
    /// Allows to quit minigame and reset when pressing Q
    /// </summary>
    private void Update()
    {
        if (isInMinigame && Input.GetKeyDown(KeyCode.Q)){
        EndMinigame();
    }
        // Reset the scene when pressing R
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
            StartMinigame();
        }
    }

    private void StartMinigame()
    {
        isInMinigame = true;
        Camera.main.transform.position = anchorPosition;
        Camera.main.transform.rotation = anchorRotation;
        cameraMovementScript.enabled = false;
        cameraRotationScript.enabled = false;
        playerMovementScript.enabled = false;
    }

    private void EndMinigame()
    {
        isInMinigame = false;
        Reset();
        cameraMovementScript.enabled = true;
        cameraRotationScript.enabled = true;
        playerMovementScript.enabled = true;
    }

    private void Reset()
    {
        // Disable the movement scripts temporarily
        cameraMovementScript.enabled = false;  
        cameraRotationScript.enabled = false;
        playerMovementScript.enabled = false;

        // Reset player & camera position and rotation
        playerParent.transform.position = originalPlayerPosition;
        playerParent.transform.rotation = originalPlayerRotation;
        Camera.main.transform.position = originalCameraPosition;
        Camera.main.transform.rotation = originalCameraRotation;

        // Reset rotation values to prevent player-camer offset
        cameraRotationScript.velocity = Vector2.zero; 
        cameraRotationScript.frameVelocity = Vector2.zero;

        cameraMovementScript.enabled = true;  
        cameraRotationScript.enabled = true; 
        playerMovementScript.enabled = true;
    }




}