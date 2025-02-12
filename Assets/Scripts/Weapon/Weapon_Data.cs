using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon System/Weapon Data")]
public class Weapon_Data : ScriptableObject 
{
    public string weaponName;

    [Header("Bullet info")]
    public int bulletDamage;

    [Header("Magazine details")]
    public int bullletInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Header("Regular shot")]
    public ShootType shootType;
    public int bulletsPerShot =1;
    public float fireRate;

    [Header("Burst shot")]
    public bool burstAvalible;
    public bool burstActive;
    public int burstBulletPerShot;
    public float burstFireRate;
    public float burstFireDelay =.1f;

    [Header("Spread")]
    public float baseSpread;
    public float maximumSpread;
    public float spreadIncreaseRate = .15f;

    [Header("Weapon generics")]
    public WeaponType weaponType;

    [Range(1,3)]
    public float reloadSpeed = 1;
    [Range(1,3)]
    public float equipmentSpeed = 1;
    [Range(2,12)]
    public float gundistance = 4;
    [Range(3,8)]
    public float camreaDistance = 6;

}

