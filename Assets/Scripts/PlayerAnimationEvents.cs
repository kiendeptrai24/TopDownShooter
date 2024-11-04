using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerAnimationEvents : MonoBehaviour
{
    private WeaponVisualCcontroller visualCcontroller;
    private void Start() {
        visualCcontroller = GetComponentInParent<WeaponVisualCcontroller>();
    }
    public void ReloadIsOver()
    {
        visualCcontroller.MaximizeRigWeigth();
    }
    public void ReturnRig()
    {
        visualCcontroller.MaximizeRigWeigth();
        visualCcontroller.MaximizeLeftHadWeigth();
    }
    public void WeaponGrabIsOver()
    {
        visualCcontroller.SetBusyGrabbingWeaponTo(false);
    }
}
