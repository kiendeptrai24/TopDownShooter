


using System;
using UnityEngine;
using Random = UnityEngine.Random;


public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle,
}
public enum ShootType
{
    Single,
    Auto
}
[System.Serializable]
public class Weapon
{
    public WeaponType weaponType;
    [Header("Shooting spesifics")]
    public ShootType shootType;
    public float fireRate = 1;
    private float LastShootTime;

    [Header("Magazine details")]
    public int bullletInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Range(1,3)]
    public float reloadSpeed = 1;
    [Range(1,3)]
    public float equipmentSpeed = 1;

    [Header("Spread")]
    public float baseSpread;
    public float currentSpread;
    public float maximumSpread;

    public float spreadIncreaseRate = .15f;
    private float lastSpreadUpdateTime;
    public float spreadCooldown =1;
    
    #region Spread methods
    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        SpreadUpdate();
        float randomizeedValue = Random.Range(-currentSpread, currentSpread);
        Quaternion spreadRotation = Quaternion.Euler(randomizeedValue, randomizeedValue,randomizeedValue);

        return spreadRotation * originalDirection;
    }
    private void SpreadUpdate()
    {
        if(Time.time > lastSpreadUpdateTime + spreadCooldown)
            currentSpread = baseSpread;
        else
            IncreaseSpread();
            
        lastSpreadUpdateTime = Time.time;
    }
    private void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread,maximumSpread);
    }
        
    #endregion


    public bool CanShoot()
    {
        if(HaveEnoughBullets() && ReadyToFire())
        {
            bullletInMagazine--;
            return true;
        }
        return false;
    } 
    private bool ReadyToFire()
    {
        // fire fer second 1/fireRate
        if(Time.time > LastShootTime+1/fireRate)
        {
            LastShootTime = Time.time;
            return true;
        }
        return false;
    }



    #region Reload method
        
    public bool HaveEnoughBullets() => bullletInMagazine >0;
    public bool CanReload()
    {
        if(bullletInMagazine == magazineCapacity)
            return false;
        if(totalReserveAmmo > 0)
            return true;
        return false;
    }
    public void RefillBullets()
    {
        //return bullet to totalReserveAmmo
        totalReserveAmmo +=bullletInMagazine;

        int bulletToReload = magazineCapacity;
        if(bulletToReload > totalReserveAmmo)
        {
            bulletToReload =totalReserveAmmo;
        }

            
        totalReserveAmmo -= bulletToReload;
        bullletInMagazine = bulletToReload;

        if(totalReserveAmmo<0)
            totalReserveAmmo = 0;
    }

    #endregion 

}

 