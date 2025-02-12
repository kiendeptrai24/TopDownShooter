using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitBox : MonoBehaviour, IDamagable
{

    protected virtual void Awake()
    {

    }
    public virtual void TakeDamage(int damage)
    {

    }
}
