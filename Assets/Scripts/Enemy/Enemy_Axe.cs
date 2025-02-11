using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class EnemyAxe : MonoBehaviour
{
    [SerializeField] private GameObject impactFx;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform axeVisual;
    private float flySpeed;
    private float rotationSpeed =1600;

    private Transform player;
    private float timer =1;
    private Vector3 direction;
    public void SetupAxe(float flySpeed, Transform player, float timer)
    {
        this.flySpeed = flySpeed;
        this.player = player;
        this.timer = timer;
    }
    
    private void Update() {

        timer -= Time.deltaTime;
        axeVisual.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        if(timer > 0)
            direction = player.position + Vector3.up - axeVisual.position;
        transform.forward = rb.velocity;
    }
    private void FixedUpdate() {
        // it will be smoother
        rb.velocity = direction.normalized * flySpeed;
        
    }
    private void OnCollisionEnter(Collision other) {
        StrategyDamage.InvokeDamage(other.gameObject);

        GameObject newFx = ObjectPool.Instance.GetObject(impactFx,transform);
        ObjectPool.Instance.ReturnObject(gameObject);
        ObjectPool.Instance.ReturnObject(newFx,1f);

    }

}
