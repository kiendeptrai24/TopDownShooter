using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SelectedWeaponWindow : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private UI_WeaponSelection weaponSelectionUI;
    [Header("Background")]
    private Image background;
    private Sprite defaultImage;
    [SerializeField] private Sprite focusImage;

    [Header("Weapon info")]
    public Weapon_Data weaponData;
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private Image weaponIconSmall;
    [SerializeField] private TextMeshProUGUI weaponInfo;
    private void Awake() {
        background = GetComponent<Image>();
        defaultImage = background.sprite;
        weaponSelectionUI = GetComponentInParent<UI_WeaponSelection>();
        weaponData = null;
        UpdateSlotInfo(null);

    }
    
    public void UpdateSlotInfo(Weapon_Data weaponData)
    {
        if(weaponData == null)
        {
            weaponName.text = string.Empty;
            weaponIcon.color = Color.clear;
            weaponIconSmall.color = Color.clear;
            weaponInfo.text = "Select a weapon ...";  
            return;
        }
        weaponName.text = weaponData.weaponName;
        weaponIcon.color = Color.white;
        weaponIcon.sprite = weaponData.weaponIcon;
        weaponIconSmall.color = Color.white;
        weaponIconSmall.sprite = weaponData.weaponIconSmall;
        weaponInfo.text = weaponData.weaponInfo;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        weaponSelectionUI.GetWeponWindow(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        weaponData = null;
        UpdateSlotInfo(null);
        weaponSelectionUI.GetWeponWindow(this);
    }

    public void SetWeaponSlot(Weapon_Data newWeaponData)
    {
        this.weaponData = newWeaponData;
        UpdateSlotInfo(newWeaponData);
    }
    public bool IsEmpty() => weaponData == null;
    public void Reset() => background.sprite = defaultImage;
    public void ResetWindowIfNull() => weaponSelectionUI.GetWeponWindow(this);
    public void SetFocusBackground() => background.sprite = focusImage;
}
