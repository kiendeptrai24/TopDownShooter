using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Car_HealthController : MonoBehaviour, IDamagable
{
    private Car_Controller carController;
    private IExplosive explosionEffects;

    public int maxHealth;
    public int currentHealth;
    private bool carBroken;

    private void Start() {
        carController = GetComponent<Car_Controller>(); 
        explosionEffects = GetComponent<IExplosive>();
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
        carController.rb.drag = 1f;
        carController.BrakeTheCar();
        OnDestroyed();
    }
    public void OnDestroyed() => explosionEffects?.TriggerExplosion();
    public void UpdateCarHealthUI()
    {
        UI.Instance.inGameUI.UpdateCarHealthUI(currentHealth,maxHealth);
    }
    

}
