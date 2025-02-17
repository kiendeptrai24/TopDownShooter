using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, IDamagable
{
    public int currentHealth;
    public int maxHealth =100;
    [Header("Material")]
    public MeshRenderer mesh;
    public Material whiteMat;
    public Material redMat;
    private void Start() => Refresh();
    [Space]
    public float refreshCooldown;
    private float lastTimeDamage;
    private void Awake() {
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
    [ContextMenu("rotation y 65")]
    public void RorationY()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles +  new Vector3(0, 90, 0));
    }
    [ContextMenu("rotation z 75")]
    public void RorationZ()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles +  new Vector3(0, 0, 90));
    }
    private void Refresh()
    {
        currentHealth = maxHealth;
        mesh.sharedMaterial = whiteMat;
    }
    private void Update()
    {
        if(Time.time > refreshCooldown + lastTimeDamage)
            Refresh();
    }

    public void TakeDamage(int damage)
    {
        lastTimeDamage = Time.time;
        currentHealth -= damage;
        if(currentHealth <= 0)
        Die();
    }

    private void Die()
    {
        mesh.sharedMaterial =redMat;
    }
}
