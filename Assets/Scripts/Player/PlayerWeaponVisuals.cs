using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public enum GrabType{SideGrab, BackGrab};

public class PlayerWeaponVisuals : MonoBehaviour
{
    private Animator anim;
    private bool isGrabbingWeapon;

    
    #region Gun transform region
    [SerializeField] private Transform[] gunTransfroms;
    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;

    [SerializeField] private Transform autoRifle;

    [SerializeField] private Transform shotgun;

    [SerializeField] private Transform rifle;
    private Transform currentGun;
    #endregion
   
   
   
    [Header("Rig")]
    [SerializeField] private float rigWeigthIncreaseRate;
    private bool shouldIncreaseRigWiegth;
    private Rig rig;
    [Header("Left hand IK")]
    [SerializeField] private Transform leftHandIKIncreaseWeigthRate;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private float leftHandIKIncreaseStep;
    private bool shouldIncreaseLeftHandIKWeigth;
     
    private void Start() {
        anim = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        SwitchOn(pistol);
    }
    private void Update()
    {
        CheckWeaponSwitch();
        UpdateRigWigth();
        UpdateLeftHandIKWeigth();
    }

    public void PlayReloadAnimation()
    {
        if(isGrabbingWeapon)
            return;
        anim.SetTrigger("Reload");
        ReduceRightWeight();
    }

    private void UpdateLeftHandIKWeigth()
    {
        //move left hand to original
        if (shouldIncreaseLeftHandIKWeigth)
        {
            leftHandIK.weight += leftHandIKIncreaseStep * Time.deltaTime;
            if (leftHandIK.weight >= 1)
            {
                shouldIncreaseLeftHandIKWeigth = false;
            }
        }
    }

    private void UpdateRigWigth()
    {
        //move right hand to original
        if (shouldIncreaseRigWiegth)
        {
            rig.weight += rigWeigthIncreaseRate * Time.deltaTime;
            if (rig.weight >= 1)
            {
                shouldIncreaseRigWiegth = false;
            }
        }
    }

    private void ReduceRightWeight()
    {
        rig.weight = .15f;
    }

    private void PlayerWeaponGrabAnimation(GrabType _grabType)
    {
        // Grab Weapon when player is grabbing it will not do everything else
        leftHandIK.weight = 0;
        ReduceRightWeight();
        anim.SetFloat("WeaponGrabType", (float)_grabType);
        anim.SetTrigger("WeaponGrab");
        SetBusyGrabbingWeaponTo(true);
    }
    public void SetBusyGrabbingWeaponTo(bool _busy)
    {
        // if player is busy player will not do any other Animation
        isGrabbingWeapon= _busy;
        anim.SetBool("BusyGrabbingWeapon", isGrabbingWeapon);
    }

    private void SwitchOn(Transform gunTranform)
    {
        //chancing Weapon
        SwitchOfFun();
        gunTranform.gameObject.SetActive(true);
        currentGun = gunTranform;
        AttackLeftHand();
    }

    private void SwitchOfFun()
    {
        for (int i = 0; i < gunTransfroms.Length; i++)
        {
            gunTransfroms[i].gameObject.SetActive(false);
        }
    }
    private void AttackLeftHand()
    {
        //chancing leftHand through each Weapon
        Transform targetTransform = currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;
        leftHandIKIncreaseWeigthRate.localPosition = targetTransform.localPosition;
        leftHandIKIncreaseWeigthRate.localRotation = targetTransform.localRotation;
    }
    private void SwitchAnimationLayer(int _layerIndex)
    {
        // turn off all weapon ad then turn on one specific
        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i,0);
        }
        anim.SetLayerWeight(_layerIndex,1);
    }
    private void CheckWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(pistol);
            SwitchAnimationLayer(1);
            PlayerWeaponGrabAnimation(GrabType.SideGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(revolver);
            SwitchAnimationLayer(1);
            PlayerWeaponGrabAnimation(GrabType.SideGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn(autoRifle);
            SwitchAnimationLayer(1);
            PlayerWeaponGrabAnimation(GrabType.BackGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn(shotgun);
            SwitchAnimationLayer(2);
            PlayerWeaponGrabAnimation(GrabType.BackGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOn(rifle);
            SwitchAnimationLayer(3);
            PlayerWeaponGrabAnimation(GrabType.BackGrab);
        }
   
    }
    public void MaximizeRigWeigth() => shouldIncreaseRigWiegth = true;
    public void MaximizeLeftHadWeigth() => shouldIncreaseLeftHandIKWeigth = true;
}

