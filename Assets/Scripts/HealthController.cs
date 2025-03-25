using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Profiling;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    
    private bool isDead;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }
    public virtual void ReduceHealth(int damage)
    {
        currentHealth -= damage;
    }
    public virtual void IncreaseHealth()
    {
        if(currentHealth > maxHealth)
            return;
        currentHealth++;
    }
    public bool ShouldDie()
    {
        if(isDead)
            return false;
        if(currentHealth <= 0)
        {
            isDead = true;
            return true;
        }
        return false;
    }
}
