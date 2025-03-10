using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerWeaponVisuals visualController;
    private Player_WeaponController weaponController; 
    private void Start() {
        visualController = GetComponentInParent<PlayerWeaponVisuals>();
        weaponController = GetComponentInParent<Player_WeaponController>();
    }
    public void ReloadIsOver()
    {
        visualController.MaximizeRigWeigth();
        weaponController.CurrentWeapon().RefillBullets();
        weaponController.SetWeaponReady(true);
        weaponController.UpdateWeaponUI();
    }
    public void ReturnRig()
    {
        visualController.MaximizeRigWeigth();
        visualController.MaximizeLeftHadWeigth();
    }
    public void WeaponEquipingIsOver()
    {
        weaponController.SetWeaponReady(true);
    }
    public void SwitchOnWeaponModel() => visualController.SwitchOnCurrentWeaponModel();
}
