using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;

public class UI_WeaponSelection : MonoBehaviour
{
    [SerializeField] private GameObject nextUIToSwitchOn;
    public UI_SelectedWeaponWindow[] selectedWeaponWindow;
    private int indexWeaponWindow;
    private bool checkWeaponWindow = false;
    private void Start()
    {
        selectedWeaponWindow = GetComponentsInChildren<UI_SelectedWeaponWindow>();
        SetWeaponWindowBackground();
    }
    public UI_SelectedWeaponWindow GetEmptySlot()
    {
        // if user click on window 
        if(checkWeaponWindow == true)
        {
            return selectedWeaponWindow[indexWeaponWindow];
        }

        // auto find window empty
        foreach (var slot in selectedWeaponWindow)
        {
            if (slot.IsEmpty())
            {
                return slot;
            }
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
        ResetBackgroundToDefault();
        for (int i = 0; i < selectedWeaponWindow.Length; i++)
        {
            if (selectedWeaponWindow[i] == weaponWindow)
            {
                indexWeaponWindow = i;
                selectedWeaponWindow[i].SetFocusBackground();
                checkWeaponWindow = true;
            }
        }
    }
    //Give weapons for the player
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

    //reset all window to default background
    private void ResetBackgroundToDefault()
    {
        foreach (var weaponWindow in selectedWeaponWindow)
        {
            weaponWindow.Reset();
        }
    }
    // find window to set
    public void SetWeaponWindowBackground()
    {
        //get window from slot
        UI_SelectedWeaponWindow window = GetEmptySlot();

        //if window not equal to null reset background and set focus background
        if(window != null)
        {
            ResetBackgroundToDefault();
            window.SetFocusBackground();
        }
    }
    public void ComfirmWeaponSelection()
    {
        if(AtLeastOneWeaponSelection())
        {
            UI.Instance.SwitchTo(nextUIToSwitchOn);
            UI.Instance.StartLevelGeneration();
        }

        
    }
    private bool AtLeastOneWeaponSelection() => GetSelectedWeapons().Count > 0;

}
