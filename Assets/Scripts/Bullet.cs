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
    private LayerMask allyLayerMask;

    protected virtual void Awake() {
        cd = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();

    }
 
    public void BulletSetup(LayerMask _allyLayerMask, float _flyDistance = 100, float impactForce = 100)
    {
        this.impactForce = _flyDistance;
        this.allyLayerMask = _allyLayerMask;
        trailRenderer.Clear();
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
        if(FriendlyFireEnable() == false)
        {
            if((allyLayerMask.value & (1 << other.gameObject.layer)) > 0)
            {
                ReturnBulletToPool(10);
                return;
            }
        }
        CreateImpactFx();
        ReturnBulletToPool();
        //take damgage
        StrategyDamage.InvokeDamage(other.gameObject);
        
        ApplyBulletImpactToEnemy(other);

    }

    private void ApplyBulletImpactToEnemy(Collision other)
    {
        Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            Vector3 force = rb.velocity.normalized * impactForce;
            Rigidbody hitrb = other.collider.attachedRigidbody;

            enemy.BulletImpact(force, other.contacts[0].point, hitrb);

        }
    }

    protected void ReturnBulletToPool(float delay = 0) => ObjectPool.Instance.ReturnObject(gameObject, delay);
    

    private void Rebase()
    {
        bulletDisabled = false;
        cd.enabled = true;
        meshRenderer.enabled = true;
        trailRenderer.time = .5f;
    }

    protected void CreateImpactFx()
    {
        GameObject newImpactFx = ObjectPool.Instance.GetObject(bulletImpactFX,transform);
        ObjectPool.Instance.ReturnObject(newImpactFx, 1);
        
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
    public bool FriendlyFireEnable() => GameManager.Instance.friendlyFire;
}
