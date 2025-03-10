using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WeaponSlotsTransparentOnHover : UI_TransparentOnHover
{
    private bool hasUIWeaponSlots;
    private Player_WeaponController playerWeaponController; 


    protected override void Start()
    {
        base.Start();
        hasUIWeaponSlots = GetComponentInChildren<UI_WeaponSlot>() != null;
        if(hasUIWeaponSlots)
            playerWeaponController = FindObjectOfType<Player_WeaponController>();
    }
    public override void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }
    public override void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        playerWeaponController?.UpdateWeaponUI();
    }
}
