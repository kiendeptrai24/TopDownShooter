using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIFaceToCamera : MonoBehaviour
{
    
    [SerializeField] private Transform cameraToLookAt;
    private void Update() {
        gameObject.transform.LookAt(cameraToLookAt);
    }
}
