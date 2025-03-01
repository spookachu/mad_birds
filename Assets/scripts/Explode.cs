using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explode : Projectile
{
    public GameObject explosionEffect;
    private List<GameObject> boxes = new List<GameObject>();

    public override void Start()
    {
        base.Start();
        boxes.AddRange(GameObject.FindGameObjectsWithTag("Box"));
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            ExplodeNearbyBoxes();
            return;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(DisappearWithPuff());
        }
    }

    void ExplodeNearbyBoxes()
    {
        foreach (GameObject box in boxes)
        {
            if (box != null && Vector3.Distance(transform.position, box.transform.position) < 2f)
            {
                if (explosionEffect != null)
                {
                    GameObject explosion = Instantiate(explosionEffect, box.transform.position, Quaternion.identity);
                    Destroy(explosion, 1f);
                }
                Destroy(box);
            }
        }
        ResetProjectile();
    }
}
