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
    public float explosionForce = 15f;
    private bool explosionActivated = false;
    
    private bool isLaunched = false;
    private float timeAfterLaunch = 0f;
    public Text countdownText;
    public LifeManager livesManager;
    public PowerUpManager PowerUpManager;

    void Update()
    {
        if (isLaunched){
            timeAfterLaunch += Time.deltaTime;
        }

        // Trigger powerup if you have any
        if (isLaunched && !explosionActivated && Input.GetKeyDown(KeyCode.Space))
        {
            if (PowerUpManager.Instance.HasPowerUp(PowerUpType.Explosion))
            {
                StartCoroutine(StartExplosionCountdown());
                PowerUpManager.Instance.UsePowerUp(PowerUpType.Explosion);
            }
        }
    }

    public override void Start()
    {
        base.Start();

        // Store the boxes and their original positions to reset post destruction
        GameObject[] allBoxes = GameObject.FindGameObjectsWithTag("Box");
        foreach (var box in allBoxes){
            boxes.Add(box);
            boxPositions.Add(box.transform.position);
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            ExplodeNearbyBoxes();
            return;
        }

        // Let bird flop on ground for a while to have time to trigger explosion
        if (collision.gameObject.CompareTag("Ground")){
            if (timeAfterLaunch > 0.5f)
            {
                StartCoroutine(DisappearWithPuff());
            }
        }
    }

    IEnumerator StartExplosionCountdown()
    {
        explosionActivated = true;
        
        for (int i = 3; i > 0; i--)
        {
            if (countdownText != null){
                countdownText.text = "Explosion in: " + i;
            }
            yield return new WaitForSeconds(1f);
        }

        if (countdownText != null){
            countdownText.text = "";  // Clear text 
        }
        applyExplode();
    }

    /// <summary>
    /// Generate an explosion on collision with boxes.
    /// </summary>
    void applyExplode()
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 2f);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearby in colliders)
        {
            Rigidbody rb = nearby.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            if (nearby.CompareTag("Box")){
                Destroy(nearby.gameObject);
            }
        }
        Destroy(gameObject);
        StartCoroutine(ResetAfterExplosion());
    }

    public override void OnMouseUp()
    {
        if (!isThrown)
        {
            isLaunched = true;
            base.OnMouseUp();
            livesManager.UseLife();
        }
    }

    void ExplodeNearbyBoxes()
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
        foreach (GameObject box in destroyedBoxes){
            boxes.Remove(box);
        }

        CheckWinCondition();
        if (livesManager.currentLives >= 0){
            ResetProjectile();
        }
        else{
            base.BlockProjectile();
            livesManager.GameOver();

            // check if user wants to go again
            bool restart = RestartGame();
            if(restart == true){
                Start();
            }
        }
    }

    /// <summary>
    /// Verify if all boxes have been destroyed to see if game is won
    /// </summary>
    void CheckWinCondition()
    {
        if (boxes.Count == 0)
        {
            Debug.Log("Congrats, you've won BombsAway! Power-Up Earned :)");
            PowerUpManager.Instance.EarnPowerUp(PowerUpType.Explosion);
            RespawnBoxes();
            livesManager.WinGame();
        }
    }

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
