using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New weapon data", menuName = "Enemy Data/Range Weapon Data", order = 1)]
public class Enemy_RangeWeaponData : ScriptableObject {
    [Header("Weapon details")]
    public Enemy_RangeWeaponType weaponType;
    public float fireRate =1f;

    public int minBulletsPerAttack = 1;
    public int maxBulletsPerAttack = 1;

    public float minWeaponCooldown = 2;
    public float maxWeaponCooldown = 3;

    [Header("Bullet details")]
    public float bulletSpeed = 20f;
    public float weaponSpread = .1f;

    public int GetBulletsPerAttack() => Random.Range(minBulletsPerAttack,maxBulletsPerAttack);
    public float GetWeaponCooldown() => Random.Range(minWeaponCooldown,maxWeaponCooldown);
    public Vector3 ApplyWeaponSpread(Vector3 originalDirection)
    {
        float randomizeedValue = Random.Range(-weaponSpread, weaponSpread);
        Quaternion spreadRotation = Quaternion.Euler(randomizeedValue, randomizeedValue,randomizeedValue);

        return spreadRotation * originalDirection;
    }
}


