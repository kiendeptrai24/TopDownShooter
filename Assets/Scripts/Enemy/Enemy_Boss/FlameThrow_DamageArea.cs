using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FlameThrow_DamageArea : MonoBehaviour
{
    private Enemy_Boss enemy;
    private float damageCooldown;
    private float lastTimeDamage;
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy_Boss>();
        damageCooldown = enemy.flameDamageCooldown;
    }
    private void OnTriggerStay(Collider other) {
        if(enemy.flamethrowActive == false)
            return;

        if(Time.time < lastTimeDamage + damageCooldown)
            return;

        if(StrategyDamage.InvokeDamage(other.gameObject))
            lastTimeDamage = Time.time;
        
    }
    
}
