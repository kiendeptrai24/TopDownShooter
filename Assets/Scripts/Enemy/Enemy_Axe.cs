using System.Collections;
using System.Collections.Generic;
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
    public void SetupAxe(float flySpeed, Transform player,float timer)
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

        rb.velocity = direction.normalized * flySpeed;

        transform.forward = rb.velocity;
    }
    private void OnTriggerEnter(Collider other) {
        Bullet bullet= other.GetComponent<Bullet>();
        Player player= other.GetComponent<Player>();
        if(bullet != null || player != null)
        {
            GameObject newFx = ObjectPool.Instance.GetObject(impactFx);
            newFx.transform.position =gameObject.transform.position;

            ObjectPool.Instance.ReturnObject(gameObject);
            ObjectPool.Instance.ReturnObject(newFx,1f);

        }
    }
}