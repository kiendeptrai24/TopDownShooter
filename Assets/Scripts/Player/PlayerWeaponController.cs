using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsAlly;
    private const float REFERENCE_BULLET_SPEED = 20;

    private Player player;
    [SerializeField] private Weapon_Data defaultWeaponData;
    [SerializeField] private Weapon currentWeapon;
    private bool weaponReady;
    private bool isShooting;



    [Header("Bullet details")]
    [SerializeField] private float bulletImpactForce = 100;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;



    [Header("Inventory")]
    [SerializeField] int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots;

    [SerializeField] private GameObject weaponPickupPrefab;

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
    public void PickupWeapon(Weapon newWeapon)
    {

        if(WeaponInSlots(newWeapon.weaponType) != null)
        {
            WeaponInSlots(newWeapon.weaponType).totalReserveAmmo += newWeapon.bullletInMagazine;
            ///
            // int weaponIndex = weaponSlots.IndexOf(WeaponInSlots(newWeapon.weaponType));
            // EquipWeapon(weaponIndex);
            ////
            return;
        }

        if(weaponSlots.Count >= maxSlots && newWeapon.weaponType != currentWeapon.weaponType)
        {
            int weaponIndex = weaponSlots.IndexOf(currentWeapon);

            player.weaponVisuals.SwitchOffWeaponModel();
            weaponSlots[weaponIndex] = newWeapon;
            CreateWeaponOnTheGround();
            EquipWeapon(weaponIndex);

            return;
        }
        weaponSlots.Add(newWeapon);
        ///
        // int newEeaponIndex = weaponSlots.IndexOf(newWeapon);
        // EquipWeapon(newEeaponIndex);
        ///
        player.weaponVisuals.SwitchOnBackupWeaponModels();
    }
    public void WeaponStartingWeapon()
    {
        weaponSlots[0] = new Weapon(defaultWeaponData);

        EquipWeapon(0);

    } 

    
    private void EquipWeapon(int index)
    {
        if(index >= weaponSlots.Count)
            return;
        SetWeaponReady(false);
        
        currentWeapon = weaponSlots[index];
        player.weaponVisuals.PlayWeaponEquipAnimation();

        // CameraManager.Instance.ChangeCameraDistance(currentWeapon.camreaDistance);        
    }
    private void DropWeapon()
    {
        if (HasOnlyOneWeapon())
            return;

        CreateWeaponOnTheGround();

        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
    }

    private void CreateWeaponOnTheGround()
    {
        GameObject dropWeapon = ObjectPool.Instance.GetObject(weaponPickupPrefab,transform);
        dropWeapon.transform.position = transform.position;
        dropWeapon.GetComponent<Pickup_Weapon>()?.SetupPickupItem(currentWeapon, transform);
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
        if (WeaponReady() == false)
            return;

        if (currentWeapon.CanShoot() == false)
            return;

        player.weaponVisuals.PlayFireAnimation();

        if (CurrentWeapon().shootType == ShootType.Single)
            isShooting = false;


        if(currentWeapon.BurstActivated() == true)
        {
            StartCoroutine(nameof(BurstFire));
            return;
        }
        TriggerEnemyDodge();
        FireSingleBullet();

    }
    private IEnumerator BurstFire()
    {
        //setWeapon When your gun fires a burst, you get to shoot again.
        SetWeaponReady(false);
        for (int i = 1; i <= currentWeapon.bulletsPerShot; i++)
        {
            FireSingleBullet();
            yield return new WaitForSeconds(currentWeapon.burstFireDelay);

            if(i>=currentWeapon.bulletsPerShot)
                SetWeaponReady(true);
        }
    }
    private void FireSingleBullet()
    {
        currentWeapon.bullletInMagazine--;
        GameObject newBullet = ObjectPool.Instance.GetObject(bulletPrefab,GunPoint());
        
        newBullet.transform.rotation = Quaternion.LookRotation(GunPoint().forward);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.BulletSetup(whatIsAlly, currentWeapon.gundistance,bulletImpactForce);


        //bullet spread
        Vector3 bulletsDirrection = currentWeapon.ApplySpread(BulletDirection());


        rbNewBullet.mass = REFERENCE_BULLET_SPEED/bulletSpeed;
        
        rbNewBullet.velocity = bulletsDirrection * bulletSpeed;
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
    public Weapon WeaponInSlots(WeaponType weaponType)
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if(weapon.weaponType == weaponType)
                return weapon;
        }
        return null;
    }
    public Weapon CurrentWeapon() => currentWeapon;
    public Transform GunPoint() => player.weaponVisuals.CurrentWeaponModel().gunPoint;
    public void TriggerEnemyDodge()
    {
        Vector3 rayOrigin = GunPoint().position;
        Vector3 rayDirection = BulletDirection();

        if(Physics.Raycast(rayOrigin, rayDirection,out RaycastHit hit, Mathf.Infinity))
        {
            Enemy_Melee enemy_Melee = hit.collider.gameObject.GetComponentInParent<Enemy_Melee>();
            if(enemy_Melee != null)
                enemy_Melee.ActivateDodgeRoll();
        }
    }
    #region Input Event
    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;
        controls.Character.Fire.performed += context => isShooting = true;
        controls.Character.Fire.canceled += context => isShooting = false;

        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        controls.Character.EquipSlot3.performed += context => EquipWeapon(2);
        controls.Character.EquipSlot4.performed += context => EquipWeapon(3);
        controls.Character.EquipSlot5.performed += context => EquipWeapon(4);

        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();
        controls.Character.ToogleWeaponMode.performed += context => currentWeapon.ToggleBurst();

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
