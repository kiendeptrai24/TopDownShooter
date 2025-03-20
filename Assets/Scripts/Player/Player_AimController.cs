using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Aim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;
    [Header("Aim Visual - Laser")]
    [SerializeField] private LineRenderer aimLaser; // this component is on the weapon holder(child of a player)


    [Header("Aim control")]
    [SerializeField] private Transform aim;
    [SerializeField] private bool isAimingPrecisly; 
    [SerializeField] private bool isLockingToTarget;
    [Header("Camera control")]
    [SerializeField] private Transform cameraTarget;
    [Range(.5f,1f)]
    [SerializeField] private float minCameraDistance = 1f;
    [Range(1,3)]
    [SerializeField] private float maxCameraDistance = 1.5f;
    [Range(3,10)]
    [SerializeField] private float cameraSensetvity = 5f;
    

    [Space]
    [SerializeField] private LayerMask aimPlayerMask;

    [SerializeField] float x;
    [SerializeField] float y;


    private Vector2 mouseInput;
    private RaycastHit lastKnowMouseHit = new RaycastHit();

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            isAimingPrecisly = !isAimingPrecisly;
        if(Input.GetKeyDown(KeyCode.L))
            isLockingToTarget= !isLockingToTarget; 
        if(player.controlsEnable == false)
            return;
        UpdateAimVisuals();   
        UpdateAimPosition();
        UpdateCameraPosition();
    }
    public Transform GetAimCameraTarget()
    {
        cameraTarget.position = player.transform.position;
        return cameraTarget;
    }
    public void EnableAimLazer(bool enable) => aimLaser.enabled = enable;
    private void UpdateAimVisuals()
    {
        aimLaser.enabled = player.weapon.WeaponReady();

        if(aimLaser.enabled == false)
            return;
        if(player.controlsEnable == false)
            return;

        WeaponModel weaponModel= player.weaponVisuals.CurrentWeaponModel();

        weaponModel.transform.parent.LookAt(aim);
        weaponModel.gunPoint.parent.LookAt(aim);

        Transform gunPoint = player.weapon.GunPoint();
        Vector3 laserDirection = player.weapon.BulletDirection();

        float laserTipLenght =.5f;
        float gunDistance = player.weapon.CurrentWeapon().gundistance;
        Vector3 endPoint = gunPoint.position + laserDirection * gunDistance;

        if(Physics.Raycast(gunPoint.position,laserDirection,out RaycastHit hit, gunDistance))
        {
            endPoint = hit.point;
            laserTipLenght = 0;
        }
        aimLaser.SetPosition(0,gunPoint.position);
        aimLaser.SetPosition(1,endPoint);
        aimLaser.SetPosition(2,endPoint + laserDirection * laserTipLenght);
    }
    private void UpdateAimPosition()
    {
        Transform target = Target();
        if(target != null && isLockingToTarget)
        {
            if(target.GetComponent<Renderer>() != null)
                aim.position = target.GetComponent<Renderer>().bounds.center;
            else
                aim.position = target.position;
            return;
        }

        aim.position = GetMouseHitInfo().point;
        if (!isAimingPrecisly)
            aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
    }
    public Transform Target()
    {
        Transform target = null;

        if(GetMouseHitInfo().transform?.GetComponent<Target>() != null)
        {
            target = GetMouseHitInfo().transform;
        }
        return target;
    }

    public Transform Aim() => aim;
    public bool CanAimPrecisly() => isAimingPrecisly;

    public RaycastHit GetMouseHitInfo()
    {
        //getting position of the mouse
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);
        if(Physics.Raycast(ray, out RaycastHit hitInfo ,Mathf.Infinity , aimPlayerMask))
        {
            lastKnowMouseHit = hitInfo;
            return lastKnowMouseHit;
        }

        return lastKnowMouseHit;
    }
    //get position through min and max distance

    #region Camera Regoin
    private void UpdateCameraPosition()
    {
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesiredCameraPosition(), cameraSensetvity * Time.deltaTime);
    }
    private Vector3 DesiredCameraPosition()
    {
        //when player is going down maxdistance equal to mindistance  
        float actualMaxCameraDistance = player.movement.moveInput.y < -.5f ? minCameraDistance : maxCameraDistance;
        //getting direction of player to mouse
        Vector3 desiredCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;

        // assign distance for player to aim
        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);
        float clampedDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance,actualMaxCameraDistance);
        
        desiredCameraPosition = transform.position + aimDirection * clampedDistance;
        desiredCameraPosition.y = transform.position.y + 1;

        return desiredCameraPosition;
    }
    #endregion
    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Character.Aim.performed += context => mouseInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => mouseInput = Vector2.zero;
    }

}
