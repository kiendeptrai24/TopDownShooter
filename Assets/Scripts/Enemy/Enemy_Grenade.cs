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
    private LayerMask allyLayerMask;
    private bool canExplore = true;
    private void Awake() => rb = GetComponent<Rigidbody>();

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && canExplore)
        {
            Explode();
        }
    }

    private void Explode()
    {
        canExplore = false;

        PlayExplosionFx();
        HashSet<GameObject> uniqueEntities = new HashSet<GameObject>();
        Debug.Log(uniqueEntities.Count);
        Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius);
        foreach (Collider hit in colliders)
        {
            IDamagable damagable = hit.GetComponent<IDamagable>();
            if(damagable != null)
            {
                if (IsTargetValid(hit) == false)
                    continue;

                // the root which return the last parent 
                GameObject rootEntity = hit.transform.root.gameObject;
                if (uniqueEntities.Add(rootEntity) == false)
                    continue;
                    
                StrategyDamage.InvokeDamage(hit.gameObject);
            }

            ApplyPhysicalForceTo(hit);
        }
    }

    private void ApplyPhysicalForceTo(Collider hit)
    {
        Rigidbody rb = hit.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddExplosionForce(impactPower, transform.position, impactRadius, upwardsMutiplier, ForceMode.Impulse);
        }
    }

    private void PlayExplosionFx()
    {
        GameObject newFx = ObjectPool.Instance.GetObject(explosionFx, transform);
        ObjectPool.Instance.ReturnObject(newFx, .5f);
        ObjectPool.Instance.ReturnObject(gameObject);
    }

    public void SetupGrenade(LayerMask allyLayerMask, Vector3 target, float timeTarget,float countdown,float impactPower)
    {
        canExplore = true;
        
        this.allyLayerMask = allyLayerMask;
        rb.velocity = CalculateLaunchVelocity(target, timeTarget);
        timer = countdown + timeTarget;
        this.impactPower = impactPower;
    }
    private bool IsTargetValid(Collider collider)
    {
        if(GameManager.Instance.friendlyFire)
            return true;
        
        if((allyLayerMask.value & (1 << collider.gameObject.layer)) > 0)
            return false;
        return true;
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
