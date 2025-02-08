using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : HealthController
{
    private Player player; 
    public bool isDead {get; private set;}
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }
    public override void ReduceHealth()
    {
        base.ReduceHealth();
        if(ShouldIde())
            Die();
            
    }
    private void Die()
    {
        isDead = true;
        player.anim.enabled = false;
        player.ragdoll.RagdollActive(true);
    }
}  
