using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public enum Enemy_MeleeWeaponType{ OneHand, Throw, Unarmed}
public enum Enemy_RangeWeaponType{ Pistol, Revolver,  AutoRifle, Shotgun,Rifle}

public class Enemy_Visuals : MonoBehaviour
{

    public GameObject currentWeaponModel { get; private set; }
    [Header("Corruption visuals")]
    [SerializeField] private GameObject[] corruptionCrytals;
    // curruptionAmount should be less than corruptionCrytals.Length
    [SerializeField] private int corruptionAmount;
    [Header("Color")]
    [SerializeField] private Texture[] colorTextures;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    [Header("Rig references")]
    [SerializeField] private Transform leftHandIK;
    [SerializeField] private Transform leftEblowIK;
    [SerializeField] private TwoBoneIKConstraint leftHandIKConstraint;
    [SerializeField] private MultiAimConstraint weaponAimConstraint;
    

   
    public void SetupLook()
    {
        SetUpRandomColor();
        SetupRandomWeapon();
        SetupRandomCorruption();
    }
    public void EnableWeaponTrail(bool enable)
    {
        Enemy_WeaponModel currentWeaponScript = currentWeaponModel.GetComponent<Enemy_WeaponModel>();
        currentWeaponScript.EnableTrailEffect(enable);
    }
    private void SetupRandomWeapon()
    {
        bool thisEnemyIsMelee = GetComponent<Enemy_Melee>() != null;
        bool thisEnemyIsRange = GetComponent<Enemy_Range>() != null;
        if(thisEnemyIsRange)
            currentWeaponModel = FindRangeWeaponModel();
        if (thisEnemyIsMelee)
            currentWeaponModel = FindMeleeWeaponModel();


        currentWeaponModel.SetActive(true);
        OverrideAnimatorControllerIfCan();
    }

    private GameObject FindRangeWeaponModel()
    {
        Enemy_RangeWeaponModel[] weaponModels = GetComponentsInChildren<Enemy_RangeWeaponModel>(true);
        Enemy_RangeWeaponType weaponType = GetComponent<Enemy_Range>().weaponType;
        
        foreach (var weaponModel in weaponModels)
        {
            if(weaponModel.weaponType == weaponType)
            {
                SwitchAnimationLayer((int)weaponModel.HoldType);
                SetupLeftHandIK(weaponModel.leftHandTarget, weaponModel.leftElbowTarget);
                return weaponModel.gameObject;
            }
        }
        Debug.LogWarning("dont find any weapon !!!");
        return null;
    }

    private GameObject FindMeleeWeaponModel()
    {
        Enemy_WeaponModel[] weaponModels = GetComponentsInChildren<Enemy_WeaponModel>(true); 
        Enemy_MeleeWeaponType weaponType  = GetComponent<Enemy_Melee>().weaponType;
        List<Enemy_WeaponModel> filteredWeaponModel = new List<Enemy_WeaponModel>();
        
        foreach (var weaponModel in weaponModels)
            weaponModel.gameObject.SetActive(false);

        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponType == weaponType)
                filteredWeaponModel.Add(weaponModel);
        }
        int randomIndex = Random.Range(0, filteredWeaponModel.Count);
        return filteredWeaponModel[randomIndex].gameObject;
        
    }

    private void OverrideAnimatorControllerIfCan()
    {
        AnimatorOverrideController overrideController = currentWeaponModel.GetComponent<Enemy_WeaponModel>()?.overrideController;

        if (overrideController != null)
        {
            GetComponentInChildren<Animator>().runtimeAnimatorController = overrideController;
        }
    }

    private void SetupRandomCorruption()
    {
        List<int> avalibleIndexs = new List<int>();
        corruptionCrytals = CollectCorruptionCrytals();
        for (int i = 0; i < corruptionCrytals.Length; i++)
        {
            avalibleIndexs.Add(i);
            corruptionCrytals[i].SetActive(false);
        }
        for (int i = 0; i < corruptionAmount; i++)
        {
            if(avalibleIndexs.Count == 0)
                break;
            int randomIndex = Random.Range(0,avalibleIndexs.Count);
            int objectIndex = avalibleIndexs[randomIndex];
            corruptionCrytals[objectIndex].SetActive(true);
            avalibleIndexs.RemoveAt(randomIndex);
        }
    }
    private void SetUpRandomColor()
    {
        int randomIndex = Random.Range(0, colorTextures.Length);
        Material newMat = new Material(skinnedMeshRenderer.material);
        newMat.mainTexture = colorTextures[randomIndex];
        skinnedMeshRenderer.material = newMat;
    }
    private GameObject[] CollectCorruptionCrytals()
    {
        Enemy_CorruptionCrytal[] crytalComponents = GetComponentsInChildren<Enemy_CorruptionCrytal>(true);
        GameObject[] corruptionCrytals = new GameObject[crytalComponents.Length];

        for (int i = 0; i < crytalComponents.Length; i++)
        {
            corruptionCrytals[i] = crytalComponents[i].gameObject;
        }
        return corruptionCrytals;
    }
    private void SwitchAnimationLayer(int _layerIndex)
    {
        Animator anim = GetComponentInChildren<Animator>();
        // turn off all layer weapon and then turn on one specific
        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i,0);
        }
        anim.SetLayerWeight(_layerIndex,1);
    }
    public void EnableIK(bool enableLeftHand, bool enableAim)
    {
        leftHandIKConstraint.weight = enableLeftHand ? 1 : 0;
        weaponAimConstraint.weight = enableAim ? 1 : 0;
    }
    public void SetupLeftHandIK(Transform leftHandTarget,Transform leftEblowTarget)
    {
        leftHandIK.localPosition = leftHandTarget.localPosition;
        leftHandIK.localRotation = leftHandTarget.localRotation;

        leftEblowIK.localPosition = leftEblowTarget.localPosition;
        leftEblowIK.localRotation = leftEblowTarget.localRotation;

    }

}
