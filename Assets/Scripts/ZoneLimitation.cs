using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using UnityEngine;

public class ZoneLimitation : MonoBehaviour
{
    private ParticleSystem[] lines;
    public BoxCollider zoneColliders { get; set; }
    private void Start() {
        GetComponent<MeshRenderer>().enabled = false;
        zoneColliders = GetComponent<BoxCollider>();
        lines = GetComponentsInChildren<ParticleSystem>();
        ActivateWall(false);
    }
    private void ActivateWall(bool active)
    {
        foreach (var line in lines)
        {
            if(active)
            {
                line.Play();
            }
            else{
                line.Stop();
            }
        }
        zoneColliders.isTrigger = !active;
    }
    IEnumerator WallActivation()
    {
        ActivateWall(true);
        yield return new WaitForSeconds(3);
        ActivateWall(false);

    }
    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(WallActivation());
        Debug.Log("My sensors are going crazy, I think it's dangerous!");
    }
}
