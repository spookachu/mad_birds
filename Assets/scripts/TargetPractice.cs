using UnityEngine;
using System.Collections;

public class TargetPractice : Projectile
{
    public GameObject projectile;
    private float timeAfterLaunch = 0f;
    public GameObject groundObject;
    private bool isOnGround = false;
    public GameObject Target1, Target2, Target3;
    public LifeManager livesManager;
    public PowerUpManager PowerUpManager;

    
    //Effects
    public GameObject confettiEffect;
    private bool isSizeDoubled = false; 

    public override void Update()
    {
        if (isThrown)
        {   
            timeAfterLaunch += Time.deltaTime;
            if (isOnGround || timeAfterLaunch > 3f)
            {
                StartCoroutine(DisappearWithPuff());
                isThrown = false;
                timeAfterLaunch = 0f;
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

        if (collision.gameObject == Target1 || collision.gameObject == Target2 || collision.gameObject == Target3)
        {
            StartCoroutine(TriggerEffect(confettiEffect));

            if (collision.gameObject == Target1) Target1 = null;
            if (collision.gameObject == Target2) Target2 = null;
            if (collision.gameObject == Target3) Target3 = null;

            CheckWinCondition();
            return;
        }
    }

    IEnumerator TriggerEffect(GameObject effectPrefab)
    {
        livesManager.UseLife();
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
            livesManager.totalLives = 3;
            base.ResetProjectile();
        }
    }

    void CheckWinCondition()
    {
        if (Target1 == null && Target2 == null && Target3 == null)
        {
            Debug.Log("TargetPractice Won! Power-Up Earned.");
            PowerUpManager.Instance?.EarnPowerUp(PowerUpType.SizeIncrease);
            livesManager.WinGame();

            //reset
            base.ResetProjectile();
            livesManager.totalLives = 3;
        }
    }

    void DoubleProjectileSize()
    {
        projectile.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f); 
        isSizeDoubled = true;
        Debug.Log("Projectile size doubled!");
    }
}
 