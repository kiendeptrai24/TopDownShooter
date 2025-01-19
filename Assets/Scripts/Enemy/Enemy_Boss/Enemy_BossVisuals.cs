using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossVisuals : MonoBehaviour
{
    private Enemy_Boss enemy;
    [SerializeField] private GameObject[] batteries;
    [SerializeField] private float initalBatteryScaleY = .2f;
    private float dischargeSpeed;
    private float rechargeSpeed;
    private bool isRecharging;
    private void Awake() {
        enemy = GetComponent<Enemy_Boss>();
        ResetBatteries();
    }
    private void Update() {
        UpdateBatteriesScale();
    }
    private void UpdateBatteriesScale()
    {
        if(batteries.Length <= 0)
            return;
        foreach(GameObject battery in batteries)
        {
            if(battery.activeSelf)
            {
                float scaleChange = (isRecharging ? rechargeSpeed  : -dischargeSpeed) * Time.deltaTime;
                float newScaleY = Mathf.Clamp(battery.transform.localScale.y + scaleChange ,0,initalBatteryScaleY);
                battery.transform.localScale = new Vector3(.15f, newScaleY , .15f);
                if(battery.transform.localScale.y <= 0)
                    battery.SetActive(false);
            }            
        }
    }
    public void ResetBatteries()
    {
        isRecharging = true;
        rechargeSpeed = initalBatteryScaleY / enemy.abilityCooldown;
        dischargeSpeed = initalBatteryScaleY / (enemy.flamethrowDuration * .75f);

        foreach (GameObject battery in batteries)
        {
            battery.SetActive(true);
        }

    }
    public void dischargeBatteries() => isRecharging = false;
}
