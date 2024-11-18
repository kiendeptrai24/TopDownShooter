


using System;
using UnityEngine;
using UnityEngine.Rendering;

public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle,
}
[System.Serializable]
public class Weapon
{
    public WeaponType weaponType;
    public int bullletInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Range(1,3)]
    public float reloadSpeed = 1;
    [Range(1,3)]
    public float equipmentSpeed = 1;

    [Space]
    public float fireRate = 1;
    private float LastShootTime;

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

 