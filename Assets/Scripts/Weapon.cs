


using System;

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
    public bool CanShoot()
    {
        return HaveEnoughBullets();
    } 
    public bool HaveEnoughBullets()
    {

        if(bullletInMagazine > 0)
        {
            bullletInMagazine--;
            return true;    
        }
        return false;
    }
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
}

 