using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy_DropController : MonoBehaviour
{
    [SerializeField] private GameObject missionObjectKey;
    public void GiveKey(GameObject newKey) => missionObjectKey = newKey;
    public void DropItem()
    {
        if(missionObjectKey != null)
            CreateItem(missionObjectKey);
        Debug.Log("Drop some items");
    }
    private void CreateItem(GameObject go)
    {
        Instantiate(go, transform.position + Vector3.up, Quaternion.identity);
    }
}
