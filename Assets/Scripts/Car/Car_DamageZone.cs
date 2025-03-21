using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using UnityEngine;

public class Car_DamageZone : MonoBehaviour
{
    private Car_Controller carController;
    [SerializeField] private float minSpeedToDamage = 1.5f; 
    [SerializeField] private int carDamage;
    [SerializeField] private float impactForce = 150;
    [SerializeField] private float upwardMultiplier = 3;
    private void Awake() {
        carController = GetComponentInParent<Car_Controller>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(carController.rb.velocity.magnitude > minSpeedToDamage)
            return;

        IDamagable damagable = other.GetComponent<IDamagable>();
        damagable?.TakeDamage(GetDamageBySpeed());
        
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if(rigidbody != null && enemy != null)
        {
            StartCoroutine(TestForce(enemy,rigidbody));
        }
    }
    private IEnumerator TestForce(Enemy enemy,Rigidbody rigidbody)
    {
        enemy.anim.enabled = false;
        enemy.agent.enabled = false;
        enemy.ragdoll.RagdollActive(true);
        ApplyForce(rigidbody);

        yield return new WaitForSeconds(2);

        enemy.anim.enabled = true;
        enemy.agent.enabled = true;
        enemy.ragdoll.RagdollActive(false);

    }
    private int GetDamageBySpeed()
    {
        int damage = Mathf.RoundToInt(carController.rb.velocity.magnitude * carDamage * .7f);
        return damage;
    }
    private void ApplyForce(Rigidbody rigidbody)
    {
        
       // rigidbody.isKinematic = false;
        rigidbody.AddExplosionForce(impactForce,transform.position ,3 , upwardMultiplier,ForceMode.Impulse);
    }

}
