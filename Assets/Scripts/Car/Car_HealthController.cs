using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_HealthController : MonoBehaviour, IDamagable
{
    private Car_Controller carController;
    public int maxHealth;
    public int currentHealth;
    private bool carBroken;
    private void Start() {
        carController = GetComponent<Car_Controller>(); 
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        if(carBroken)
            return;

        ReduceHealth(damage);
        UpdateCarHealthUI();
    }
    private void ReduceHealth(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
            BrakeTheCar();
    }

    private void BrakeTheCar()
    {
        carBroken = true;
        carController.BrakeTheCar();
    }
    public void UpdateCarHealthUI()
    {
        UI.Instance.inGameUI.UpdateCarHealthUI(currentHealth,maxHealth);
    }
    
}
