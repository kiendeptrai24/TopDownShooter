using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float impactForce;

    private BoxCollider cd;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private TrailRenderer trailRenderer;
    [SerializeField] private GameObject bulletImpactFX;

    private Vector3 startPosition;
    private float flyDistance;
    private bool bulletDisabled;

    protected virtual void Awake() {
        cd = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();

    }
 
    public void BulletSetup(float _flyDistance = 100, float impactForce = 100)
    {
        this.impactForce = _flyDistance;

        startPosition = transform.position;
        this.flyDistance = _flyDistance +.5f;
        Rebase();
    }
    protected virtual void Update()
    {
        FadeTrailIfNeeded();
        DisableBulletIfNeeded();
        ReturnToPoolIfNeeded();

    }
    protected virtual void OnCollisionEnter(Collision other)
    {
        CreateImpactFx(other);
        ReturnBulletToPool();

        Enemy enemy= other.gameObject.GetComponentInParent<Enemy>();
        EnemyShield shield = other.gameObject.GetComponent<EnemyShield>();
        if(shield != null)
        {
            shield.ReduceDurability();
            return;
        }
        if(enemy != null)
        {
            Vector3 force = rb.velocity.normalized * impactForce;            
            Rigidbody hitrb = other.collider.attachedRigidbody;

            enemy.GetHit();
            enemy.DeadImpact(force, other.contacts[0].point, hitrb);

        }


    }

    protected void ReturnBulletToPool() => ObjectPool.Instance.ReturnObject(gameObject);
    

    private void Rebase()
    {
        bulletDisabled = false;
        cd.enabled = true;
        meshRenderer.enabled = true;
        trailRenderer.time = .5f;
    }

    protected void CreateImpactFx(Collision other)
    {
        if (other.contacts.Length > 0)
        {
            ContactPoint contact = other.contacts[0];

            GameObject newImpactFx = ObjectPool.Instance.GetObject(bulletImpactFX);
            newImpactFx.transform.position = contact.point;
            ObjectPool.Instance.ReturnObject(newImpactFx, 1);
        }
    }
    protected void ReturnToPoolIfNeeded()
    {
        if (trailRenderer.time < 0)
            ReturnBulletToPool();

    }

    protected void DisableBulletIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && !bulletDisabled)
        {
            cd.enabled = false;
            meshRenderer.enabled = false;
            bulletDisabled = true;
        }
    }

    protected void FadeTrailIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5)
            trailRenderer.time -= 5 * Time.deltaTime;
    }
}
