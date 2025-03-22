using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance {get; private set;}
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer transposer;
    [Header("Camera distance")]
    [SerializeField] private bool canChangeCameraDistance;
    [SerializeField] private float distanceChangeRate;
    private float targetCameraDistance;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }
    private void Update()
    {
        UpdateCameraDistance();
    }

    private void UpdateCameraDistance()
    {
        if(canChangeCameraDistance == false)
            return;
        float currentDistance = transposer.m_CameraDistance;

        if (MathF.Abs(targetCameraDistance - currentDistance) < 0.01f)
            return;
        
        transposer.m_CameraDistance = Mathf.Lerp(transposer.m_CameraDistance, targetCameraDistance, distanceChangeRate * Time.deltaTime);
    }

    public void ChangeCameraDistance(float distance) => targetCameraDistance = distance; 
    public void ChangeCameraTarget(Transform target,float cameraDistance = 10, float newLookAheadTime = 0)
    {
        virtualCamera.Follow = target;
        transposer.m_LookaheadTime = newLookAheadTime;
        ChangeCameraDistance(cameraDistance);
    }
}
