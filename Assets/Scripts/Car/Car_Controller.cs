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
    public float turnSensitivity;
    public float speed;
    [Header("Car Settings")]
    [SerializeField] private Transform centerOfMass;

    [Header("Engine Settings")]
    public float currentSpeed;
    [Range(7,12)]
    public float maxSpeed;
    [Range(.5f,5)]
    public float accelerationSpeed;
    [Range(1500,3000)]
    public float motorForce = 1500f;
    [Header("Brake Settings")]
    [Range(4,10)]
    public float brakeSensetivity =5;
    public float brakePower = 5000;
    private bool isBraking;

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
    }
    private void FixedUpdate() {
        CarControls?.Invoke();
        ApplyAnimattionToWheel();
        ApplyDrive();
        ApplySteering();
        ApplyBrake();
        ApplySpeedLimit();
    }


    private void ApplyBrake()
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

    private void ApplyDrive()
    {
        float newBrakeTorque = brakePower * brakeSensetivity * Time.deltaTime;
        float currentBrakeTorque = isBraking ? newBrakeTorque : 0;
        foreach (var wheel in wheels)
        {
            if(wheel.axelType == AxelType.Front)
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
        controls.Car.Brake.performed += ctx => isBraking = true;
        controls.Car.Brake.canceled += ctx => isBraking = false;

    }
}
