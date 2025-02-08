using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StrategyDamage
{
    private static IDamagable damagable;
    public static void InvokeDamage(GameObject collider)
    {
        damagable = collider.GetComponent<IDamagable>();
        damagable?.TakeDamage();
    }
}
