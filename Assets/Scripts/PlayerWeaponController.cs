using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;

    [SerializeField] private Transform weaponHolder;
    private void Start() {
        player = GetComponent<Player>();
        player.controls.Character.Fire.performed += context => Shoot();
    }
    private void Shoot()
    {

        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position,Quaternion.LookRotation(gunPoint.forward));
        newBullet.GetComponent<Rigidbody>().velocity = BulletDirection() * bulletSpeed;
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
    public Transform GunPoint() => gunPoint;
    // private void OnDrawGizmos() {
    //     Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25);
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawLine(gunPoint.position,gunPoint.position + BulletDirection() * 25);
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(new Vector3(transform.position.x,transform.position.y + 1, transform.position.z), transform.position + transform.forward * 25);

    // }

}
