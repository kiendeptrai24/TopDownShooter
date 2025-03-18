using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Car_Controller : MonoBehaviour
{
    public static Car_Controller Instance {get; private set;}
    private PlayerControls controls;
    private Rigidbody rb;

    private float moveInput;
    private float steerInput;
    [Range(30,60)]
    public float turnSensitivity;
    public float speed;
    [Header("Car Settings")]
    [SerializeField] private Transform centerOfMass;

    [Header("Engine Settings")]
    [SerializeField] private float currentSpeed;
    [Range(7,40)]
    [SerializeField] private float maxSpeed;
    [Range(.5f,10)]
    [SerializeField] private float accelerationSpeed;
    [Range(1500,3000)]
    [SerializeField] private float motorForce = 1500f;

    [Header("Brake Settings")]
    [Range(4,10)]
    [SerializeField] private float brakeSensetivity =5;
    [SerializeField] private float brakePower = 5000;
    private bool isBraking;

    [Header("Drift settings")]
    [Range(0,1)]
    [SerializeField] private float frontDriftFactor = .5f;
    [Range(0,1)]
    [SerializeField] private float backDriftFactor = .5f;
    [SerializeField] private float driftDuration = 1f;
    private float driftTimer;

    private Car_Wheel[] wheels;
    public Action CarControls {get; private set;}

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
        controls = ControlsManager.Instance.controls;
        ControlsManager.Instance.SwitchToCarControls();
        wheels = GetComponentsInChildren<Car_Wheel>();
        AssignInputEvents();
    }
    private void Update() {
        speed = rb.velocity.magnitude;
        driftTimer -= Time.deltaTime;
        if(driftTimer < 0)
            isBraking = false;
    }
    private void FixedUpdate() {
        CarControls?.Invoke();
        ApplyAnimattionToWheel();
        ApplyDrive();
        ApplySteering();
        ApplyBrake();
        ApplySpeedLimit();
        if(isBraking)
            ApplyDrift();
        else
            StopDrift();
    }


    private void ApplyDrive()
    {
        currentSpeed = moveInput * accelerationSpeed * Time.deltaTime;

        float motorTorqueValue = motorForce * currentSpeed;
        foreach (var wheel in wheels)
        {
            if(wheel.axelType == AxelType.Back)
            {
                wheel.cd.motorTorque = motorTorqueValue;
            }
        }
    }
    private void ApplyDrift()
    {
        foreach (var wheel in wheels)
        {
            bool frontWheel = wheel.axelType == AxelType.Front;
            float driftFactor =  frontWheel ? frontDriftFactor : backDriftFactor;

            WheelFrictionCurve sidewaysFriction = wheel.cd.sidewaysFriction;
            
            sidewaysFriction.stiffness *= (1 - driftFactor);
            wheel.cd.sidewaysFriction = sidewaysFriction;

        }
    }
    private void StopDrift()
    {
        foreach (var wheel in wheels)
        {
            wheel.RestoreDefaultStiffness();
        }
    }
    private void ApplySpeedLimit()
    {
        if(rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;
    }

    private void ApplySteering()
    {
        foreach (var wheel in wheels)
        {
            if(wheel.axelType == AxelType.Front)
            {
                float targetSteerAngle = steerInput * turnSensitivity;
                wheel.cd.steerAngle = Mathf.Lerp(wheel.cd.steerAngle, targetSteerAngle, .5f);
            }
        }
    }

    private void ApplyBrake()
    {
        float newBrakeTorque = brakePower * brakeSensetivity * Time.deltaTime;
        float currentBrakeTorque = isBraking ? newBrakeTorque : 0;
        foreach (var wheel in wheels)
        {
            if(wheel.axelType == AxelType.Back)
                wheel.cd.brakeTorque = currentBrakeTorque;
        }
    }
    private void ApplyAnimattionToWheel()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rotation;
            Vector3 position;
            wheel.cd.GetWorldPose(out position,out rotation);
            if(wheel.model != null)
            {
                wheel.model.transform.position = position;
                wheel.model.transform.rotation = rotation;

            }
        }
    }

    private void AssignInputEvents()
    {
        controls.Car.Movement.performed += ctx =>
        {
            Vector2 input  = ctx.ReadValue<Vector2>();
            moveInput = input.y;
            steerInput = input.x;
        };
        controls.Car.Movement.canceled += ctx =>
        {
            moveInput = 0;
            steerInput = 0;
        };
        controls.Car.Brake.performed += ctx =>
        {
            isBraking = true;
            driftTimer = driftDuration;
        };
            
        controls.Car.Brake.canceled += ctx => isBraking = false;

    }
}
