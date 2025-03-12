using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_WeaponSelectionButton : UI_Button
{
    private UI_WeaponSelection weaponSelectionUI;
    [SerializeField] private Weapon_Data weaponData;
    [SerializeField] private Image weaponIcon;
    private UI_SelectedWeaponWindow emptySlot;
    private void OnValidate() {
        gameObject.name = "Button - Select Weapon: " +weaponData.weaponType;
    }
    protected override void Start()
    {
        base.Start();
        weaponSelectionUI = GetComponentInParent<UI_WeaponSelection>();
        weaponIcon.sprite = weaponData.weaponIcon;
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        weaponSelectionUI.SetWeaponWindowBackground();
        emptySlot = weaponSelectionUI.GetEmptySlot();
        emptySlot?.UpdateSlotInfo(weaponData);
            
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        weaponSelectionUI.SetWeaponWindowBackground();
        
        if(weaponSelectionUI.WeaponWindowIsHas())
        {
            if(emptySlot?.weaponData != null)
            {
                emptySlot?.UpdateSlotInfo(emptySlot.weaponData); 
                return;
            }
        }
        emptySlot?.UpdateSlotInfo(null);
        emptySlot = null;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        UI_SelectedWeaponWindow SlotBusyThisWeapon = weaponSelectionUI.FindSlowWeaponOfType(weaponData);
        if(SlotBusyThisWeapon != null)
        {
            SlotBusyThisWeapon.SetWeaponSlot(null);
            weaponSelectionUI.SetWeaponWindowBackground();

        }
        else
        {
            emptySlot = weaponSelectionUI.GetEmptySlot();
            emptySlot?.SetWeaponSlot(weaponData);
            weaponSelectionUI.SetWeaponWindowBackground();

            FindWindow();

        }

        emptySlot = null;

    }

    private void FindWindow()
    {
        weaponSelectionUI.SetCheckWeaponWindow(false);
        if (weaponSelectionUI.GetEmptySlot() == null)
            emptySlot.ResetWindowIfNull();
    }
}
