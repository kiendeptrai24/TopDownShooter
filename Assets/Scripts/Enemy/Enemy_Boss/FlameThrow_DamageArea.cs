
using UnityEngine;

public class FlameThrow_DamageArea : MonoBehaviour
{
    private Enemy_Boss enemy;
    private float damageCooldown;
    private float lastTimeDamage;
    private int flameDamage;
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy_Boss>();
        damageCooldown = enemy.flameDamageCooldown;
        flameDamage = enemy.flameDamage;
    }
    private void OnTriggerStay(Collider other) {
        IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
        if(enemy.flamethrowActive == false)
            return;

        if(Time.time < lastTimeDamage + damageCooldown)
            return;

        if(damagable != null)
        {
            damagable.TakeDamage(flameDamage);
            lastTimeDamage = Time.time;
        }
        
    }
    
}
