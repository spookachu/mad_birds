using UnityEngine;
using System.Collections;

public class TargetPractice : Projectile
{
    public GameObject projectile;
    private float timeAfterLaunch = 0f;
    public GameObject groundObject;
    private bool isOnGround = false;
    private GameObject[] targets;
    public LifeManager livesManager;
    public PowerUpManager PowerUpManager;

    
    //Effects & levels
    public GameObject confettiEffect;
    private bool isSizeDoubled = false; 
    public GameObject level1Set;
    public GameObject level2Set;

    public override void Update()
    {
        targets = GameObject.FindGameObjectsWithTag("Target");

        if (isThrown)
        {   
            timeAfterLaunch += Time.deltaTime;
            if (isOnGround || timeAfterLaunch > 3f)
            {
                StartCoroutine(DisappearWithPuff());
                isThrown = false;
                timeAfterLaunch = 0f;
                livesManager.UseLife();
            }

            if (PowerUpManager.Instance.HasPowerUp(PowerUpType.SizeIncrease) && Input.GetKeyDown(KeyCode.Space) && !isSizeDoubled){
                DoubleProjectileSize();
            }
        }
    }
   
    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == groundObject)
        {
            isOnGround = true;
            StartCoroutine(DisappearWithPuff()); 
        }

        if (collision.gameObject.CompareTag("Target"))
        {
            StartCoroutine(TriggerEffect(confettiEffect));
            collision.gameObject.SetActive(false);
            CheckWinCondition();
        }
    }

    IEnumerator TriggerEffect(GameObject effectPrefab)
    {
        
        if (effectPrefab != null)
        {
            GameObject effectInstance = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            ParticleSystem effectParticleSystem = effectInstance.GetComponent<ParticleSystem>();
            if (effectParticleSystem != null)
            {
                effectParticleSystem.Play();
                yield return new WaitForSeconds(1.5f);
                Destroy(effectInstance);
            }
        }
        if (livesManager.currentLives > 0){
            base.ResetProjectile();
         }
        else{
            livesManager.GameOver();

            // check if user wants to go again
            livesManager.RestartGame();
            base.ResetProjectile();
            foreach (GameObject target in targets)
            {
                target.SetActive(true);
            }
        }
    }

    void CheckWinCondition()
    {
        foreach (GameObject target in targets)
        {
            if (target.activeSelf) return;
        }
        Debug.Log("TargetPractice Won! Power-Up Earned.");
        PowerUpManager.Instance?.EarnPowerUp(PowerUpType.SizeIncrease);
        livesManager.WinGame();

        //reset
        GameReset();
    }

    void GameReset()
    {
        base.ResetProjectile();
        livesManager.totalLives = 3;

        // move to next level
        level1Set.SetActive(false);
        level2Set.SetActive(true); 
    }
   
    void DoubleProjectileSize()
    {
        projectile.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f); 
        isSizeDoubled = true;
        Debug.Log("Projectile size doubled!");
    }
}
 