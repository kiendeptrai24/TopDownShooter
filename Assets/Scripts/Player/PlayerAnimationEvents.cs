using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerWeaponVisuals visualController;
    private PlayerWeaponController weaponController; 
    private void Start() {
        visualController = GetComponentInParent<PlayerWeaponVisuals>();
        weaponController = GetComponentInParent<PlayerWeaponController>();
    }
    public void ReloadIsOver()
    {
        visualController.MaximizeRigWeigth();
        weaponController.CurrentWeapon().RefillBullets();
    }
    public void ReturnRig()
    {
        visualController.MaximizeRigWeigth();
        visualController.MaximizeLeftHadWeigth();
    }
    public void WeaponGrabIsOver()
    {
        visualController.SetBusyGrabbingWeaponTo(false);
    }
    public void SwitchOnWeaponModel() => visualController.SwitchOnCurrentWeaponModel();
}
