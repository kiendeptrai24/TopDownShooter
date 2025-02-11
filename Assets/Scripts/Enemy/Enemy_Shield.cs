using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour, IDamagable
{
    private Enemy_Melee enemy;
    [SerializeField] private int durability;
    private void Awake() {
        enemy = GetComponentInParent<Enemy_Melee>();
        durability = enemy.shieldDurability;
    }
    public void ReduceDurability()
    {
        Debug.Log("Dsadasda");
        durability--;
        if(durability <= 0)
        {
            enemy.anim.SetFloat("ChaseIndex", 0);
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage()
    {
        ReduceDurability();
    }
}
