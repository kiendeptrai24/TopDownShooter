using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance { get; private set; }
    public event Action<float, float> OnHealthChanged;
    public event Action<List<Weapon>, Weapon> OnWeaponChanged;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateHealthUI(float currentHealth, float maxHealth) => OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
    public void UpdateWeaponUI(List<Weapon> weapons, Weapon currentWeapon) => OnWeaponChanged?.Invoke(weapons, currentWeapon);
    
    
}
