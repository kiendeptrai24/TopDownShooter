using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarExplosionEffects : MonoBehaviour, IExplosive
{
    private Car_Controller carController;

    [Header("Explosion info")]
    [SerializeField] private int explosionDamage = 350;
    [Space]
    [SerializeField] private float explosionRadius = 3;
    [SerializeField] private float explosionDelay = 3;
    [SerializeField] private float explosionForce = 7;
    [SerializeField] private float explosionUpwardsModifer = 2;
    [Space]
    [SerializeField] private ParticleSystem fire;
    [SerializeField] private ParticleSystem explosion;
    private void Start() {
        carController = GetComponent<Car_Controller>();
    }
    private void Update() {
        if(fire.gameObject.activeSelf)
            fire.transform.rotation = Quaternion.identity;
    }
    public void TriggerExplosion()
    {
        fire.gameObject.SetActive(true);
        StartCoroutine(ExplosionDelay(explosionDelay));
    }
    public void ApplyExplosionDamage()
    {
        HashSet<GameObject> unieqEnties = new HashSet<GameObject>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            IDamagable damagable = hit.GetComponent<IDamagable>();
            if(damagable != null)
            {
                GameObject rootEntity = hit.transform.root.gameObject;
                if(unieqEnties.Add(rootEntity) == false)
                    continue;
                damagable.TakeDamage(explosionDamage);

                Vector3 explosionPoint = transform.position + Vector3.forward * 1.5f;

                hit.GetComponentInChildren<Rigidbody>().
                    AddExplosionForce(explosionForce,explosionPoint,explosionRadius,explosionUpwardsModifer,ForceMode.VelocityChange);
            }
        }
    }
    private IEnumerator ExplosionDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        explosion.gameObject.SetActive(true);
        carController.rb.constraints = RigidbodyConstraints.None;
        carController.rb.
            AddExplosionForce(explosionForce,transform.position - Vector3.down + (Vector3.forward * 1.5f)
            ,explosionRadius, explosionUpwardsModifer,ForceMode.Impulse);
        ApplyExplosionDamage();
    }

}
