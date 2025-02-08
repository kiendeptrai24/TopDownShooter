using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Profiling;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    


    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }
    public virtual void ReduceHealth()
    {
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            return;
        }
        currentHealth--;
    }
    public virtual void IncreaseHealth()
    {
        if(currentHealth > maxHealth)
            return;
        currentHealth++;
    }
    public bool ShouldIde() => currentHealth <= 0;
}
