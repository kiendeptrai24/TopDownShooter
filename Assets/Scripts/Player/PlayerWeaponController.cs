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

    [Header("Bullet details")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;


    [SerializeField] private Transform weaponHolder;

    [Header("Inventory")]
    [SerializeField] int maxSlots =2;
    [SerializeField] private List<Weapon> weaponSlots;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
        currentWeapon.bullletInMagazine = currentWeapon.totalReserveAmmo;
    }

    #region Slot Mangement Pickup\Equip\Drop Weapon
    public void PickupItem(Weapon newWeapon)
    {
        if(weaponSlots.Count >= maxSlots)
        {
            Debug.Log("No slot available");
            return;
        }
        weaponSlots.Add(newWeapon);
    }
    private void EquipWeapon(int index)
    {
        currentWeapon = weaponSlots[index];
    }
    private void DropWeapon()
    {
        if(weaponSlots.Count <= 1)
            return;
        weaponSlots.Remove(currentWeapon);
        currentWeapon = weaponSlots[0];
    }
    
        
    #endregion
    private void Shoot()
    {
        if(currentWeapon.CanShoot() == false)
            return;
        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position,Quaternion.LookRotation(gunPoint.forward));

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();  
        rbNewBullet.mass = REFERENCE_BULLET_SPEED/bulletSpeed;
        rbNewBullet.velocity = BulletDirection() * bulletSpeed;
        Destroy(newBullet,3);
        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }
    public Vector3 BulletDirection()
    {
        Transform aim = player.aim.Aim();
        Vector3 direction = (aim.position - gunPoint.position).normalized;
        //boolean onGizmos error
        if(player.aim.CanAimPrecisly() == false && player.aim.Target() == null)
            direction.y = 0;

        //TODO: find a better place for it
        // weaponHolder.LookAt(aim);
        // gunPoint.LookAt(aim);

        return direction;
    }
    public Weapon CurrentWeapon() => currentWeapon;
    public Transform GunPoint() => gunPoint;

    #region Input Event
    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;
        controls.Character.Fire.performed += context => Shoot();
        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();
        controls.Character.Reload.performed += context =>
        {
            if(currentWeapon.CanReload())
                player.weaponVisuals.PlayReloadAnimation();
        };

    }
        
    #endregion

}
