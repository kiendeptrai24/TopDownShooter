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
        if(rigidbody != null)
            ApplyForce(rigidbody);
    }
    
    private int GetDamageBySpeed()
    {
        int damage = Mathf.RoundToInt(carController.rb.velocity.magnitude * carDamage * .7f);
        return damage;
    }
    private void ApplyForce(Rigidbody rigidbody)
    {
        rigidbody.AddExplosionForce(impactForce,transform.position ,3 , upwardMultiplier,ForceMode.Impulse);
    }

}
