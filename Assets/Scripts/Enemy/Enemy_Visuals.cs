using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;
public enum Enemy_MeleeWeaponType{ OneHand, Throw, Unarmed}
public class Enemy_Visuals : MonoBehaviour
{
    [Header("Weapon visual")]
    [SerializeField]  private Enemy_WeaponModel[] weaponModels;
    [SerializeField] private Enemy_MeleeWeaponType weaponType;
    public GameObject currentWeaponModel { get; private set; }
    [Header("Corruption visuals")]
    [SerializeField] private GameObject[] corruptionCrytals;
    // curruptionAmount should be less than corruptionCrytals.Length
    [SerializeField] private int corruptionAmount;
    [Header("Color")]
    [SerializeField] private Texture[] colorTextures;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    private void Awake()
    {
        weaponModels = GetComponentsInChildren<Enemy_WeaponModel>(true);
        CollectCorruptionCrytals();

        InvokeRepeating(nameof(SetupLook), 0, 1.5f);
    }

   
    public void SetupLook()
    {
        SetUpRandomColor();
        SetupRandomWeapon();
        SetupRandomCorruption();
    }
    public void SetupWeaponType(Enemy_MeleeWeaponType type) => weaponType = type;
    public void EnableWeaponTrail(bool enable)
    {
        Enemy_WeaponModel currentWeaponScript= currentWeaponModel.GetComponent<Enemy_WeaponModel>();
        currentWeaponScript.EnableTrailEffect(enable);
    }
    private void SetupRandomWeapon()
    {
        foreach (Enemy_WeaponModel weapon in weaponModels)
        {
            weapon.gameObject.SetActive(false);
        }
        List<Enemy_WeaponModel> filteredWeaponModel = new List<Enemy_WeaponModel>();

        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponType == weaponType)
                filteredWeaponModel.Add(weaponModel);
        }
        int randomIndex = Random.Range(0, filteredWeaponModel.Count);
        currentWeaponModel = filteredWeaponModel[randomIndex].gameObject;
        currentWeaponModel.SetActive(true);
        OverrideAnimatorControllerIfCan();
    }

    private void OverrideAnimatorControllerIfCan()
    {
        AnimatorOverrideController overrideController = currentWeaponModel.GetComponent<Enemy_WeaponModel>().overrideController;

        if (overrideController != null)
        {
            GetComponentInChildren<Animator>().runtimeAnimatorController = overrideController;
        }
    }

    private void SetupRandomCorruption()
    {
        List<int> avalibleIndexs = new List<int>();
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
    private void CollectCorruptionCrytals()
    {
        Enemy_CorruptionCrytal[] crytalComponents = GetComponentsInChildren<Enemy_CorruptionCrytal>(true);
        corruptionCrytals = new GameObject[crytalComponents.Length];

        for (int i = 0; i < crytalComponents.Length; i++)
        {
            corruptionCrytals[i] = crytalComponents[i].gameObject;
        }
    }

}
