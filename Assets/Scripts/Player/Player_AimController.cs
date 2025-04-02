using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Aim : MonoBehaviour
{
    private CameraManager cameraManager;
    private Player player;
    private PlayerControls controls;
    [Header("Aim Visual - Laser")]
    [SerializeField] private LineRenderer aimLaser; // this component is on the weapon holder(child of a player)


    [Header("Aim Control")]
    [SerializeField] private float preciseAimCamDistance = 6;
    [SerializeField] private float regulerAimCamDistance = 7;
    [SerializeField] private float canChangeRate = 5;



    [Header("Aim Setup")]
    [SerializeField] private Transform aim;
    [SerializeField] private bool isAimingPrecisly; 
    [SerializeField] private float offsetChangeRate = 6;
    private float offsetY;


    [Header("Aim Layers")]
    [SerializeField] private LayerMask preciseAim;
    [SerializeField] private LayerMask regularAim;



    [Header("Camera Control")]
    [SerializeField] private Transform cameraTarget;
    [Range(.5f,1f)]
    [SerializeField] private float minCameraDistance = 1f;
    [Range(1,3)]
    [SerializeField] private float maxCameraDistance = 1.5f;
    [Range(3,10)]
    [SerializeField] private float cameraSensetvity = 5f;
    

    [Space]
    [SerializeField] private LayerMask aimPlayerMask;


    private Vector2 mouseInput;
    private RaycastHit lastKnowMouseHit = new RaycastHit();

    private void Start()
    {
        cameraManager = CameraManager.Instance;
        player = GetComponent<Player>();
        AssignInputEvents();
    }
    private void Update()
    {
        if(player.controlsEnable == false)
            return;
        UpdateAimVisuals();   
        UpdateAimPosition();
        UpdateCameraPosition();
    }
    private void EnablePrecisesAim(bool enable)
    {
        isAimingPrecisly = !isAimingPrecisly;
        if(enable)
        {
            cameraManager.ChangeCameraDistance(preciseAimCamDistance, canChangeRate);
            Time.timeScale = 0.9f;
        }
        else{
            cameraManager.ChangeCameraDistance(regulerAimCamDistance,canChangeRate);
            Time.timeScale = 1;
        }
    }
    public Transform GetAimCameraTarget()
    {
        cameraTarget.position = player.transform.position;
        return cameraTarget;
    }
    public void EnableAimLazer(bool enable) => aimLaser.enabled = enable;
    private void UpdateAimVisuals()
    {

        aim.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
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
        aim.position = GetMouseHitInfo().point;
        Vector3 newAimPosition = isAimingPrecisly ? aim.position : transform.position;

        aim.position = new Vector3(aim.position.x, newAimPosition.y + AdjustOffsetY(), aim.position.z);
    }
    private float AdjustOffsetY()
    {
        if(isAimingPrecisly)
            offsetY = Mathf.Lerp(offsetY, 0 , Time.deltaTime * offsetChangeRate * 0.5f);
        else
            offsetY = Mathf.Lerp(offsetY,1 , Time.deltaTime * offsetChangeRate);
        return offsetY;
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
        bool canMoveCamera = Vector3.Distance(cameraTarget.position,DesiredCameraPosition()) > 1;
        if(canMoveCamera == false)
            return;
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
        controls.Character.PreciseAim.performed += context => EnablePrecisesAim(true);
        controls.Character.PreciseAim.canceled += context => EnablePrecisesAim(false);
        controls.Character.Aim.performed += context => mouseInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => mouseInput = Vector2.zero;
    }

}
