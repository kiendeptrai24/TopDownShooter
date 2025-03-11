using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WeaponSelection : MonoBehaviour
{
    public UI_SelectedWeaponWindow[] selectedWeaponWindow;
    private int indexWeaponWindow;
    private bool checkWeaponWindow = false;
    private void Start()
    {
        selectedWeaponWindow = GetComponentsInChildren<UI_SelectedWeaponWindow>();
    }
    public UI_SelectedWeaponWindow GetEmptySlot()
    {
        if(checkWeaponWindow == true)
            return selectedWeaponWindow[indexWeaponWindow];

        foreach (var slot in selectedWeaponWindow)
        {
            if (slot.IsEmpty())
                return slot;
        }
        return null;
    }
    public UI_SelectedWeaponWindow FindSlowWeaponOfType(Weapon_Data weapon_Data)
    {
        foreach (var slot in selectedWeaponWindow)
        {
            if (slot.weaponData == weapon_Data)
                return slot;
        }
        return null;
    }
    public void GetWeponWindow(UI_SelectedWeaponWindow weaponWindow)
    {
        for (int i = 0; i < selectedWeaponWindow.Length; i++)
        {
            if (selectedWeaponWindow[i] == weaponWindow)
            {
                indexWeaponWindow = i;
                checkWeaponWindow = true;
            }
        }
    }
    public List<Weapon_Data> GetSelectedWeapons()
    {
        List<Weapon_Data> selectedWeapons = new List<Weapon_Data>();
        foreach (var slot in selectedWeaponWindow)
        {
            if (!slot.IsEmpty())
                selectedWeapons.Add(slot.weaponData);
        }
        return selectedWeapons;
    }
    public void SetCheckWeaponWindow(bool check) => checkWeaponWindow = check;
    public bool WeaponWindowIsHas() => checkWeaponWindow;
    
}
