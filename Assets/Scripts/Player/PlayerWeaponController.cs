using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;

public class PlayerWeaponController : MonoBehaviour
{
    private const float REFERENCE_BULLET_SPEED = 20;

    private Player player;
    [SerializeField] private Weapon currentWeapon;
    private bool weaponReady;
    private bool isShooting;



    [Header("Bullet details")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;



    [Header("Inventory")]
    [SerializeField] int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots ;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
        currentWeapon.bullletInMagazine = currentWeapon.totalReserveAmmo;
        Invoke(nameof(WeaponStartingWeapon),.3f);
    }
    private void Update() {
        if(isShooting)
            Shoot();
    }

    #region Slot Mangement Pickup\Equip\Drop\Ready Weapon
    public void PickupItem(Weapon newWeapon)
    {
        if(weaponSlots.Count >= maxSlots)
        {
            Debug.Log("No slot available");
            return;
        }
        weaponSlots.Add(newWeapon);
        player.weaponVisuals.SwitchOnBackupWeaponModels();
    }
    public void WeaponStartingWeapon() => EquipWeapon(0);

    
    private void EquipWeapon(int index)
    {
        SetWeaponReady(false);
        
        currentWeapon = weaponSlots[index];
        player.weaponVisuals.PlayWeaponEquipAnimation();
        //change gunpoint
    }
    private void DropWeapon()
    {
        if(HasOnlyOneWeapon())
            return;
        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
    }
    public Weapon BackupWeapon()
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if(weapon != currentWeapon)
            {
                return weapon;
            }
        }
        return null;
    }
    public void SetWeaponReady(bool ready) => weaponReady = ready;
    public bool WeaponReady() => weaponReady; 
        
    #endregion
    private void Shoot()
    {
        if(WeaponReady() == false)
            return;
        if(currentWeapon.CanShoot() == false)
            return;
        if(CurrentWeapon().shootType == ShootType.Single)
            isShooting = false;
        GameObject newBullet = ObjectPool.Instance.GetBullet();

        newBullet.transform.position = GunPoint().position;
        newBullet.transform.rotation = Quaternion.LookRotation(GunPoint().forward);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();  
        //bullet spread
        Vector3 bulletsDirrection = currentWeapon.ApplySpread(BulletDirection());


        rbNewBullet.mass = REFERENCE_BULLET_SPEED/bulletSpeed;
        rbNewBullet.velocity = bulletsDirrection * bulletSpeed;
        player.weaponVisuals.PlayFireAnimation();
    }
    
    private void Reload()
    {
        SetWeaponReady(false);
        player.weaponVisuals.PlayReloadAnimation();
    }
    public Vector3 BulletDirection()
    {
        Transform aim = player.aim.Aim();
        Vector3 direction = (aim.position - GunPoint().position).normalized;
        
        if(player.aim.CanAimPrecisly() == false && player.aim.Target() == null)
            direction.y = 0;


        return direction;
    }
    

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;
    public Weapon CurrentWeapon() => currentWeapon;
    public Transform GunPoint() => player.weaponVisuals.CurrentWeaponModel().gunPoint;

    #region Input Event
    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;
        controls.Character.Fire.performed += context => isShooting = true;
        controls.Character.Fire.canceled += context => isShooting = false;

        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();
        controls.Character.Reload.performed += context =>
        {
            if(currentWeapon.CanReload() && WeaponReady())
            {
                Reload();
            }
        };

    }


    #endregion

}
