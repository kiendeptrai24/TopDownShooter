using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy_Grenade : MonoBehaviour
{
    [SerializeField] private GameObject explosionFx;
    [SerializeField] private float impactRadius;
    [SerializeField] private float upwardsMutiplier = 1; 
    private float impactPower;
    private Rigidbody rb;
    private float timer;
    private void Awake() => rb = GetComponent<Rigidbody>();

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        GameObject newFx = ObjectPool.Instance.GetObject(explosionFx);
        newFx.transform.position = transform.position;
        ObjectPool.Instance.ReturnObject(newFx,.5f);
        ObjectPool.Instance.ReturnObject(gameObject);
        Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(impactPower, transform.position, impactRadius, upwardsMutiplier, ForceMode.Impulse);
            }
        }
    }

    public void SetupGrenade(Vector3 target, float timeTarget,float countdown,float impactPower)
    {
        rb.velocity = CalculateLaunchVelocity(target, timeTarget);
        timer = countdown + timeTarget;
        this.impactPower = impactPower;
    }
    
    private Vector3 CalculateLaunchVelocity(Vector3 target, float timeTarget)
    {
        Vector3 direction = target - transform.position;
        Vector3 directionXZ = new Vector3(direction.x, 0f, direction.z);

        Vector3 velocityXZ = directionXZ / timeTarget;

        float velocityY = (direction.y  - (Physics.gravity.y * Mathf.Pow(timeTarget, 2) / 2))/timeTarget;   
        Vector3 launchVelocity = velocityXZ + Vector3.up * velocityY;
        return launchVelocity;
    }
}
