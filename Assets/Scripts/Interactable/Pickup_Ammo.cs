using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AmmoBoxType{ SmallBox, BigBox}
public class Pickup_Ammo : Interactable
{
    [SerializeField] private AmmoBoxType boxType;

    [System.Serializable]
    public struct AmmoData
    {
        public WeaponType weaponType;
        [Range(10,100)] public int minAmount;
        [Range(10,100)] public int maxAmount;
    }

    [SerializeField] private List<AmmoData> smallBoxAmmo;
    [SerializeField] private List<AmmoData> bigBoxAmmo;
    [SerializeField] private GameObject[] boxModel;

    private void Start()
    {
        SetupBoxModel();
    }

    private void SetupBoxModel()
    {
        for (int i = 0; i < boxModel.Length; i++)
        {
            boxModel[i].SetActive(false);
            if (i == (int)boxType)
            {
                boxModel[i].SetActive(true);
                UpdateMeshAndMaterial(boxModel[i].GetComponent<MeshRenderer>());
            }
        }
    }

    public override void Interaction()
    {

        List<AmmoData> currentAmmoList = smallBoxAmmo;

        if(boxType == AmmoBoxType.BigBox)
            currentAmmoList = bigBoxAmmo;

        foreach (AmmoData ammo in currentAmmoList)
        {
            Weapon weapon = weaponController.WeaponInSlots(ammo.weaponType);
            AddBulletToWeapon(weapon, GetBulletAmount(ammo));
        }
        ObjectPool.Instance.ReturnObject(gameObject);
    }
    private void AddBulletToWeapon(Weapon weapon,int amount)
    {
        if(weapon == null)
            return;
        weapon.totalReserveAmmo += amount;
    }
    private int GetBulletAmount(AmmoData ammoData)
    {
        float min = Mathf.Min(ammoData.minAmount, ammoData.maxAmount);
        float max = Mathf.Max(ammoData.minAmount, ammoData.maxAmount);

        float randomAmmoAmount = Random.Range(min, max);
        return Mathf.RoundToInt(randomAmmoAmount);
    }
    private void Reset() {
        
    }
}
