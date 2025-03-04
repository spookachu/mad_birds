using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Explode : Projectile
{
    // Box management
    private List<GameObject> boxes = new List<GameObject>();
    private List<Vector3> boxPositions = new List<Vector3>(); 
    private List<GameObject> destroyedBoxes = new List<GameObject>();

    // explosion management
    public GameObject explosionEffect;
    public float explosionRadius = 5f;
    public float explosionForce = 0.5f;
    private bool explosionActivated = false;
    
    private bool isLaunched = false;
    private float timeAfterLaunch = 0f;
    public Text countdownText;
    public LifeManager livesManager;
    public PowerUpManager PowerUpManager;
    public GameObject level1Set;
    public GameObject level2Set;
    private bool lastLevel;

    void Update()
    {
        if (isLaunched){
            timeAfterLaunch += Time.deltaTime;
        }

        // Trigger powerup if you have any
        if (!explosionActivated && Input.GetKeyDown(KeyCode.Space))
        {
            if (PowerUpManager.Instance.HasPowerUp(PowerUpType.Blast))
            {
                StartCoroutine(StartBlastCountdown());
                PowerUpManager.Instance.UsePowerUp(PowerUpType.Blast);
            }
        }
    }

    public override void Start()
    {
        base.Start();
        lastLevel = false;

        // Store the boxes and their original positions to reset post destruction
        SetupBoxes();
    }

    /// <summary>
    /// Manage behavior when projectile collides with something.
    /// Anything with the "tag" box is set to explode.
    /// If the throw misses and the projectile hits the ground, the projectile resets. 
    /// </summary>
    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            ExplodeNearbyBoxes(0.5f, explosionForce);
            livesManager.UseLife();
            return;
        }

        // Let bird flop on ground for a while to have time to trigger explosion
        if (collision.gameObject.CompareTag("Ground")){
            if (timeAfterLaunch > 0.5f)
            {
                StartCoroutine(DisappearWithPuff());
                livesManager.UseLife();
                return;
            }
        }
    }

    public override void OnMouseUp()
    {
        if (!isThrown)
        {
            isLaunched = true;
            base.OnMouseUp();
        }
    }

    /// <summary>
    /// Create a timer to announce the big blast will occur in 3 seconds.
    /// </summary>
    IEnumerator StartBlastCountdown()
    {
        explosionActivated = true;
        
        for (int i = 3; i > 0; i--)
        {
            if (countdownText != null){
                countdownText.text = "Blast in: " + i;
            }
            yield return new WaitForSeconds(1f);
        }

        if (countdownText != null){
            countdownText.text = "";  // Clear text 
        }

        // blast with a bigger radius and force
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 2f);
        }
        ExplodeNearbyBoxes(5f, explosionForce * 1.5f);
    }

    /// <summary>
    /// If projectile collides with boxes, create an explosion
    /// and check if it propagates to nearby boxes
    /// </summary>
    void ExplodeNearbyBoxes(float radius, float force)
    {
        List<GameObject> destroyedBoxes = new List<GameObject>();

        foreach (GameObject box in boxes)
        {
            if (box != null && Vector3.Distance(transform.position, box.transform.position) < 2f)
            {
                if (explosionEffect != null)
                {
                    GameObject explosion = Instantiate(explosionEffect, box.transform.position, Quaternion.identity);
                    Destroy(explosion, 1f);
                }
                destroyedBoxes.Add(box);
                Destroy(box);
            }
        }

        // clear up box gameobjects
        foreach (GameObject box in destroyedBoxes){
            boxes.Remove(box);
        }

        // adjust how many boxes in the vicinity are affected by explosion
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearby in colliders)
        {
            Rigidbody rb = nearby.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }

            if (nearby.CompareTag("Box")){
                Destroy(nearby.gameObject);
            }
        }

        // clear up box gameobjects
        foreach (GameObject box in destroyedBoxes){
            boxes.Remove(box);
        }

        CheckWinCondition();
        if (livesManager.currentLives > 0){
            ResetProjectile();
        }
        StartCoroutine(ResetAfterExplosion());
    }

    /// <summary>
    /// Verify if all boxes have been destroyed to see if game is won
    /// </summary>
    void CheckWinCondition()
    {
        if (boxes.Count == 0)
        {
            Debug.Log("Congrats, you've won BombsAway! Power-Up Earned :)");
            PowerUpManager.Instance.EarnPowerUp(PowerUpType.Blast);
            livesManager.WinGame();
            
            StartCoroutine(TransitionToNextLevel());
            lastLevel = true;
            return;
        }
        else
        {
            if (livesManager.currentLives == 0 && lastLevel== true)
            {
                livesManager.GameOver();
            }
        }
    }

    /// <summary>
    /// Add a timer in between levels switch so boxes don't get counted accidentally and trigger a game over.
    /// </summary>
    IEnumerator TransitionToNextLevel()
    {
        yield return new WaitForSeconds(2f);
        ResetProjectile();
        boxes.Clear(); 

        // Move to next level
        level1Set.SetActive(false);
        level2Set.SetActive(true);
        SetupBoxes();
    }

    // RESETING -----------------------------------
    /// <summary>
    /// Find all gameobjects with the box tag and storing them.
    /// </summary>
    void SetupBoxes()
    {
     GameObject[] allBoxes = GameObject.FindGameObjectsWithTag("Box");
        foreach (var box in allBoxes){
            boxes.Add(box);
            boxPositions.Add(box.transform.position);
        }
    }

    // RESETING -----------------------------------
    /// <summary>
    /// Recreate boxes at their original positions
    /// </summary>
    void RespawnBoxes()
    {
        for (int i = 0; i < boxPositions.Count; i++)
        {
            if (boxes.Count < boxPositions.Count) 
            {
                GameObject newBox = Instantiate(GameObject.FindGameObjectWithTag("Box"), boxPositions[i], Quaternion.identity);
                boxes.Add(newBox);
            }
        }
    }

    IEnumerator ResetAfterExplosion()
    {
        yield return new WaitForSeconds(2f);
        ResetProjectile();
    }

    public override void ResetProjectile()
    {
        base.ResetProjectile();
        isLaunched = false;
        explosionActivated = false;
        timeAfterLaunch = 0f;
    }
}