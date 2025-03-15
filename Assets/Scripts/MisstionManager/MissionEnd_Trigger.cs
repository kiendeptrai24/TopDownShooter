using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionEnd_Trigger : MonoBehaviour
{
    private GameObject player;

    private void Start() {
        player = GameObject.Find("Player");
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != player)
            return;
        if(MissionManager.Instance.MissionCompleted())
        {
            Debug.Log("Level comleted!");
            GameManager.Instance.GameCompleted();
        }
    }
}
