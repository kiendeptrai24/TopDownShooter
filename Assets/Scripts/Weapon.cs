


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

    #region Regular mode vaiable
        
    [Header("Shooting spesifics")]
    public ShootType shootType;
    public float defaultFireRate { get; private set; }
    public float fireRate;
    public int bulletsPerShot{ get; private set; }
    private float LastShootTime;
    #endregion


    #region Burst mode variable
        
    [Header("Burst fire")]
    public bool burstAvalible;
    public bool burstActive{ get; private set; }
    public int burstBulletPerShot{ get; private set; }
    public float burstFireRate{ get; private set; }
    public float burstFireDelay { get; private set; }
    #endregion


    [Header("Magazine details")]
    public int bullletInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    #region Weapon generics info variable
    public float reloadSpeed { get; private set; }
    public float equipmentSpeed { get; private set; }
    public float gundistance { get; private set; }
    public float camreaDistance { get; private set; }
    #endregion


    #region Weapon spread variable
        
    [Header("Spread")]
    public float baseSpread;
    public float currentSpread{ get; private set; }
    public float maximumSpread{ get; private set; }
    public float spreadIncreaseRate { get; private set; }
    private float lastSpreadUpdateTime;
    public float spreadCooldown { get; private set; }
    #endregion

    
    public Weapon(Weapon_Data weaponData)
    {
        bullletInMagazine = weaponData.bullletInMagazine;
        magazineCapacity = weaponData.magazineCapacity;
        totalReserveAmmo = weaponData.totalReserveAmmo;

        fireRate = weaponData.fireRate;
        weaponType = weaponData.weaponType;
        shootType = weaponData.shootType;
        bulletsPerShot = weaponData.bulletsPerShot;
        defaultFireRate = fireRate;

        baseSpread = weaponData.baseSpread;
        maximumSpread = weaponData.maximumSpread;
        spreadIncreaseRate = weaponData.spreadIncreaseRate;

        reloadSpeed = weaponData.reloadSpeed;
        equipmentSpeed = weaponData.equipmentSpeed;
        gundistance = weaponData.gundistance;
        camreaDistance = weaponData.camreaDistance;

        burstAvalible = weaponData.burstAvalible;
        burstActive = weaponData.burstActive;
        burstBulletPerShot = weaponData.burstBulletPerShot;
        burstFireRate = weaponData.burstFireRate;
        burstFireDelay = weaponData.burstFireDelay;
    }


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
    #region Burst methods
        
    public bool BurstActivated(){
        if(weaponType == WeaponType.Shotgun)
        {
            burstFireDelay = 0;
            return true;

        }
        return burstActive;
    }
    public void ToggleBurst()
    {
        if(burstAvalible == false)
            return;
        burstActive = !burstActive;

        if(burstActive)
        {
            bulletsPerShot = burstBulletPerShot;
            fireRate = burstFireRate;
        }
        else{
            bulletsPerShot = 1;
            fireRate = defaultFireRate;
        }
    }



    #endregion

    public bool CanShoot() => HaveEnoughBullets() && ReadyToFire();
    private bool ReadyToFire()
    {
        // fire fer second 1/fireRate
        if(Time.time > LastShootTime+ 1/fireRate)
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

 