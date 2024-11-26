using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class PlayerWeaponVisuals : MonoBehaviour
{
    private Player player;
    private Animator anim;

    
 
   
   [SerializeField] private WeaponModel[] weaponModels;
   [SerializeField] private BackupWeaponModel[] backupWeaponModels;
   
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
        player = GetComponent<Player>();
        anim = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        weaponModels = GetComponentsInChildren<WeaponModel>(true);
        backupWeaponModels = GetComponentsInChildren<BackupWeaponModel>(true);
    }
    private void Update()
    {
        UpdateRigWigth();
        UpdateLeftHandIKWeigth();
    }
    public void PlayFireAnimation() => anim.SetTrigger("Fire");
    public WeaponModel CurrentWeaponModel()
    {
        WeaponModel weaponModel = null;
        WeaponType weaponType = player.weapon.CurrentWeapon().weaponType; 
        foreach (WeaponModel item in weaponModels)
        {
            if(item.weaponType == weaponType)
            {
                weaponModel = item;
            }
        }
        return weaponModel;
    }

    public void PlayReloadAnimation()
    {

        float reloadSpeed = player.weapon.CurrentWeapon().reloadSpeed;
        anim.SetFloat("ReloadSpeed", reloadSpeed);
        anim.SetTrigger("Reload");
        ReduceRightWeight();
    }

    public void PlayWeaponEquipAnimation()
    {
        // Grab Weapon when player is grabbing it will not do everything else
        EquipType equipType= CurrentWeaponModel().equipType;

        float equipmentSpeed = player.weapon.CurrentWeapon().equipmentSpeed;

        leftHandIK.weight = 0;
        ReduceRightWeight();
        anim.SetFloat("EquipType", (float)equipType);
        anim.SetFloat("EquipSpeed", equipmentSpeed);
        anim.SetTrigger("EquipWeapon");
    }


    public void SwitchOnCurrentWeaponModel()
    {
        //chancing Weapon

        int animationIndex =(int)CurrentWeaponModel().holdType;
        SwitchOffWeaponModel();


        SwitchOffBackupWeaponModels();

        if(player.weapon.HasOnlyOneWeapon() == false)
            SwitchOnBackupWeaponModels();

        SwitchAnimationLayer(animationIndex);
        CurrentWeaponModel().gameObject.SetActive(true);
        AttackLeftHand();
    }
    // setactive false all weapon model
    private void SwitchOffBackupWeaponModels()
    {
        foreach (BackupWeaponModel backupModel in backupWeaponModels)
        {
            backupModel.Activate(false);
        }
    }
    // Setactive weapon model
    public void SwitchOnBackupWeaponModels()
    {
        SwitchOffWeaponModel();
        BackupWeaponModel lowHangWeapon = null;
        BackupWeaponModel backHangWeapon = null;
        BackupWeaponModel sideHangWeapon = null;
        foreach (BackupWeaponModel backupModel in backupWeaponModels)
        {
            if(backupModel.weaponType == player.weapon.CurrentWeapon().weaponType)
                continue;
            if(player.weapon.WeaponIsSlots(backupModel.weaponType) !=  null)
            {
                if(backupModel.HangTypeIs(HangType.LowBackHang))
                    lowHangWeapon = backupModel;
                if(backupModel.HangTypeIs(HangType.BackHang))
                    backHangWeapon = backupModel;
                if(backupModel.HangTypeIs(HangType.SideHang))
                    sideHangWeapon = backupModel;
            }
        }
        lowHangWeapon?.Activate(true);
        backHangWeapon?.Activate(true);
        sideHangWeapon?.Activate(true);

    }

    private void SwitchAnimationLayer(int _layerIndex)
    {
        // turn off all layer weapon and then turn on one specific
        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i,0);
        }
        anim.SetLayerWeight(_layerIndex,1);
    }
    public void SwitchOffWeaponModel()
    {
        for(int i = 0;i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false);
        }
    }


    #region Animation Rigging Methods
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
    private void AttackLeftHand()
    {
        //chancing leftHand through each Weapon
        Transform targetTransform = CurrentWeaponModel().holdPoint;
        leftHandIKIncreaseWeigthRate.localPosition = targetTransform.localPosition;
        leftHandIKIncreaseWeigthRate.localRotation = targetTransform.localRotation;
    }

    private void ReduceRightWeight()
    {
        rig.weight = .15f;
    }

    public void MaximizeRigWeigth() => shouldIncreaseRigWiegth = true;
    public void MaximizeLeftHadWeigth() => shouldIncreaseLeftHandIKWeigth = true;

    #endregion

    
}

