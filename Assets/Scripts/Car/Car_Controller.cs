using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
public enum DriveType { FrontWheelDrive, RearWheelDrive, AllWheelDrive}
[RequireComponent(typeof(Rigidbody))]
public class Car_Controller : MonoBehaviour
{
    public bool carActive {get; private set;}
    public static Car_Controller Instance {get; private set;}
    private PlayerControls controls;
    public Rigidbody rb {get; private set;}

    private float moveInput;
    private float steerInput;
    public float speed;

    [Range(30,60)]
    [SerializeField] private float turnSensitivity = 30;
    [Header("Car Settings")]
    [SerializeField] private DriveType driveType;
    [SerializeField] private Transform centerOfMass;
    [Range(350,1000)]
    [SerializeField] private float carMass = 500;
    [Range(20,80)]
    [SerializeField] private float wheelMass =30;
    [Range(.5f,2f)]
    [SerializeField] private float frontWheelTraction = 1;
    [Range(.5f,2f)]
    [SerializeField] private float backWheelTraction = 1;

    [Header("Engine Settings")]
    [SerializeField] private float currentSpeed;
    [Range(7,40)]
    [SerializeField] private float maxSpeed = 7;
    [Range(.5f,10)]
    [SerializeField] private float accelerationSpeed = 2;
    [Range(1500,7000)]
    [SerializeField] private float motorForce = 1500f;

    [Header("Brake Settings")]
    [Range(0,10)]
    [SerializeField] private float frontBrakesSensetivity =5;
    [Range(0,10)]
    [SerializeField] private float backBrakesSensetivity =5;
    [Range(2000,10000)]
    [SerializeField] private float brakePower = 5000;
    private bool isBraking;

    [Header("Drift settings")]
    [Range(0,1)]
    [SerializeField] private float frontDriftFactor = .5f;
    [Range(0,1)]
    [SerializeField] private float backDriftFactor = .5f;
    [SerializeField] private float driftDuration = 1f;
    private float driftTimer;
    private bool isDrifting;

    private Car_Wheel[] wheels;
    public Action CarControls {get; private set;}

    private void Start() {
        rb = GetComponent<Rigidbody>();
        wheels = GetComponentsInChildren<Car_Wheel>();

        controls = ControlsManager.Instance.controls;
        // ControlsManager.Instance.SwitchToCarControls();
        
        AssignInpuEtvents();
        SetupDefaultValues();
        ActivateCar(false);
    }
    private void Update()
    {
        if(carActive == false)
            return;
        speed = rb.velocity.magnitude;

        //ApplyDurationDrift();

    }

    private void ApplyDurationDrift()
    {
        driftTimer -= Time.deltaTime;
        if (driftTimer < 0)
            isDrifting = false;
    }

    private void FixedUpdate() {
        if(carActive == false)
            return;
        CarControls?.Invoke();
        ApplyAnimattionToWheel();
        ApplyDrive();
        ApplySteering();
        ApplyBrake();
        ApplySpeedLimit();
        if(isDrifting)
            ApplyDrift();
        else
            StopDrift();
    }

    private void SetupDefaultValues()
    {
        rb.centerOfMass = centerOfMass.localPosition;
        rb.mass = carMass;
        foreach (var wheel in wheels)
        {
            wheel.cd.mass = wheelMass;
            if(wheel.axelType == AxelType.Front)
                wheel.SetDefaulStiffnesst(frontWheelTraction);
            if(wheel.axelType == AxelType.Back)
                wheel.SetDefaulStiffnesst(backWheelTraction);

        }
    }
    private void ApplyDrive()
    {
        currentSpeed = moveInput * accelerationSpeed * Time.deltaTime;

        float motorTorqueValue = motorForce * currentSpeed;
        foreach (var wheel in wheels)
        {
            if(driveType == DriveType.FrontWheelDrive)
            {
                if(wheel.axelType == AxelType.Front)
                    wheel.cd.motorTorque = motorTorqueValue;
            }
            else if(driveType == DriveType.RearWheelDrive)
            {   
                if(wheel.axelType == AxelType.Back)
                    wheel.cd.motorTorque = motorTorqueValue;
            }
            else
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
        foreach (var wheel in wheels)
        {
            bool frontBrakes = wheel.axelType == AxelType.Front;
            float brakeSensetivity = frontBrakes ? frontBrakesSensetivity : backBrakesSensetivity;
            float newBrakeTorque = brakePower * brakeSensetivity * Time.deltaTime;
            float currentBrakeTorque = isBraking ? newBrakeTorque : 0;
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

    private void AssignInpuEtvents()
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
            isDrifting = true;
            driftTimer = driftDuration;
        };
            
        controls.Car.Brake.canceled += ctx =>
        {
            isBraking = false;
            isDrifting = false;
        };


        controls.Car.CarExit.performed += ctx => GetComponent<Car_Interaction>().GetOutOfTheCar();
    }
    public void ActivateCar(bool activate)
    {
        carActive = activate;
        if(activate)
            rb.constraints = RigidbodyConstraints.None;
        else
            rb.constraints = RigidbodyConstraints.FreezeAll;

    }
    
    [ContextMenu("Fucus camera and enable")]
    public void TestThisCar()
    {
        ActivateCar(true);
        CameraManager.Instance.ChangeCameraTarget(transform,12);
    }
}
