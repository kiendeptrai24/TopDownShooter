using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletImpactFX;
    private Rigidbody rb => GetComponent<Rigidbody>();

    private float timeClose = -5;
    private float timeToClose=0;
    private void OnCollisionEnter(Collision other)
    {
        CreateImpactFx(other);
        ObjectPool.Instance.ReturnBullet(gameObject);
    }
    private void Update() {
        timeToClose -= Time.deltaTime; 
        if(timeToClose <= timeClose)
        {
            ObjectPool.Instance.ReturnBullet(gameObject);
            timeToClose = 0;
        }
            
    }
    private void CreateImpactFx(Collision other)
    {
        if (other.contacts.Length > 0)
        {
            ContactPoint contact = other.contacts[0];
            GameObject newImpactFx = Instantiate(bulletImpactFX, contact.point, Quaternion.LookRotation(contact.normal));
            Destroy(newImpactFx.gameObject,1.5f);
        }
    }
}
