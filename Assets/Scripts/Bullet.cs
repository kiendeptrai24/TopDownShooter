using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BoxCollider cd;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private TrailRenderer trailRenderer;
    [SerializeField] private GameObject bulletImpactFX;

    private Vector3 startPosition;
    private float flyDistance;
    private bool bulletDisabled;

    private void Awake() {
        cd = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();

    }
 
    public void Setup(float _flyDistance)
    {
        startPosition = transform.position;
        this.flyDistance = _flyDistance +.5f;
        Rebase();
    }
    private void Update()
    {
        FadeTrailIfNeeded();
        DisableBulletIfNeeded();
        ReturnToPoolIfNeeded();

    }

    private void ReturnToPoolIfNeeded()
    {
        if (trailRenderer.time < 0)
            ReturnBulletToPool();

    }

    private void DisableBulletIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && !bulletDisabled)
        {
            cd.enabled = false;
            meshRenderer.enabled = false;
            bulletDisabled = true;
        }
    }

    private void FadeTrailIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5)
            trailRenderer.time -= 5 * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {

        CreateImpactFx(other);
        ReturnBulletToPool();
    }

    private void ReturnBulletToPool() => ObjectPool.Instance.ReturnObject(gameObject);
    

    private void Rebase()
    {
        bulletDisabled = false;
        cd.enabled = true;
        meshRenderer.enabled = true;
        trailRenderer.time = .5f;
    }

    private void CreateImpactFx(Collision other)
    {
        if (other.contacts.Length > 0)
        {
            ContactPoint contact = other.contacts[0];

            GameObject newImpactFx = ObjectPool.Instance.GetObject(bulletImpactFX);
            newImpactFx.transform.position = contact.point;
            ObjectPool.Instance.ReturnObject(newImpactFx, 1);
        }
    }
}
