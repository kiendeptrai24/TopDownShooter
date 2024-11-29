using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Pickup : MonoBehaviour
{
    [SerializeField] private Weapon_Data weaponData;
    private void OnTriggerEnter(Collider other) 
    {
        other.GetComponent<PlayerWeaponController>()?.PickupItem(weaponData);
        Destroy(gameObject);
    }
    
}
