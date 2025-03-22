using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private GameObject charactorUI;
    [SerializeField] private GameObject carUI;
    [Header("Health")]
    [SerializeField] private Image healthBar;
    [Header("Weapons")]
    [SerializeField] private UI_WeaponSlot[] weaponSlots_UI;
    [Header("Mission")]
    [SerializeField] private GameObject missionTooltip;
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private TextMeshProUGUI missionDetails;
    [Header("Car Info")]
    [SerializeField] private Image carHealthbar;
    [SerializeField] private TextMeshProUGUI carSpeedText;
    private bool tooltipActive = true;

    private void Awake() {
        weaponSlots_UI = GetComponentsInChildren<UI_WeaponSlot>();
    }
    public void SwitchToCharactorUI()
    {
        charactorUI.SetActive(true);
        carUI.SetActive(false);
    }
    public void SwitchToCarUI()
    {
        charactorUI.SetActive(false);
        carUI.SetActive(true);
    }
    public void SwitchMissionTooltip()
    {
        tooltipActive = !tooltipActive;
        missionTooltip.SetActive(tooltipActive);
    }
    public void UpdateMissionInfo(string missionText, string missionDetails = "")
    {
        this.missionText.text = missionText;
        this.missionDetails.text = missionDetails;
    }
    public void UpdateWeaponUI(List<Weapon> weaponSlots, Weapon currentWeapon)
    {
        for (int i = 0; i < weaponSlots_UI.Length; i++)
        {
            if(i < weaponSlots.Count)
            {
                bool isActiveWeapon = weaponSlots[i] == currentWeapon ? true : false;
                weaponSlots_UI[i].UpdateWeaponSlot(weaponSlots[i], isActiveWeapon);
            }
            else
            {
                weaponSlots_UI[i].UpdateWeaponSlot(null, false);
            }
        }
    }

    public void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        
    }
    public void UpdateCarHealthUI(float currentHealth, float maxHealth)
    {
        carHealthbar.fillAmount = currentHealth/maxHealth;
    }
    public void UpdateSpeedText(string text) => carSpeedText.text = text;
   
}
