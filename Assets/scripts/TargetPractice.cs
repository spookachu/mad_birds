using UnityEngine;
using System.Collections;

public class TargetPractice : Projectile
{
    private float timeAfterLaunch = 0f;

    public GameObject groundObject;
    private bool isOnGround = false;
    public GameObject Target1, Target2, Target3;
    
    //Effects
    public GameObject confettiEffect;

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
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == groundObject)
        {
            isOnGround = true;
        }

        if (collision.gameObject == Target1 || collision.gameObject == Target2 || collision.gameObject == Target3)
        {
            StartCoroutine(TriggerEffect(confettiEffect));
            return;
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
        ResetProjectile();
    }
}
