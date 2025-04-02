using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObjectKey : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float turnSpeed;
    public static event Action OnKeyPickedUp;
    private void Awake() {
        player = GameObject.Find("Player");
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != player)
            return;
        OnKeyPickedUp?.Invoke();
        Destroy(gameObject);
    }
    private void Update() {
        float directionY = turnSpeed * Time.deltaTime;
        transform.rotation *= Quaternion.Euler(0, directionY, 0);
    }
}
