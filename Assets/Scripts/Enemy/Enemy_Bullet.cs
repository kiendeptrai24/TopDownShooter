using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : Bullet
{
    protected override void OnCollisionEnter(Collision other)
    {
        Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
        if(enemy != null)
            return;
        CreateImpactFx();
        ReturnBulletToPool();

        Player player = other.gameObject.GetComponent<Player>();

        // if(player != null)
        // {
        //     Debug.Log("Fire player");
        // }
    }
}
