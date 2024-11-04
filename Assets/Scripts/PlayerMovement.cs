using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;
    private CharacterController characterController;
    private Animator anim;
    [Header("Movement info")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float speed;
    public Vector3 movementDir;
    private float verticalVeloccity;
    private bool isRunning;
    [Header("Aim info")]
    [SerializeField] private Transform aim;
    [SerializeField] private LayerMask aimPlayerMask;
    private Vector3 lookingDir;
    private Vector2 moveInput;
    private Vector2 aimInput;




    private void Start() {
        characterController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();
        speed = walkSpeed;
        AssignInputEvents();
        
    }
    private void Update()
    {
        ApplyMovement();

        AimTowardMouse();
        AnimationControllers();


    }
    private void AnimationControllers()
    {
        float xVelocity = Vector3.Dot(movementDir.normalized, transform.right);
        float zVelocity = Vector3.Dot(movementDir.normalized, transform.forward);

        anim.SetFloat("xVelocity", xVelocity,.1f,Time.deltaTime);
        anim.SetFloat("zVelocity", zVelocity,.1f,Time.deltaTime);
        bool playRunAnimation = isRunning && movementDir.magnitude > 0;
        anim.SetBool("IsRunning", playRunAnimation);

    }

    private void AimTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimPlayerMask))
        {
            lookingDir = hitInfo.point - transform.position;
            lookingDir.y = 0;
            // lookingDir.Normalize();
            // transform.forward = lookingDir;
            
            // release
            Quaternion quaternion = Quaternion.LookRotation(lookingDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quaternion,.2f);

            aim.position = new Vector3(hitInfo.point.x, transform.position.y + 1, hitInfo.point.z);
        }
    }

    private void ApplyMovement()
    {
        movementDir = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGavility();
        if (movementDir.magnitude > 0)
        {
            characterController.Move(movementDir * Time.deltaTime * speed);
        }
    }

    private void ApplyGavility()
    {
        if(characterController.isGrounded == false)
        {
            verticalVeloccity = verticalVeloccity -9.81f * Time.deltaTime;
            movementDir.y = verticalVeloccity;
        }
        else
            verticalVeloccity = -.5f;    
        
    }
    
    private void AssignInputEvents()
    {
        controls = player.controls;
        controls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Character.Movement.canceled += context => moveInput = Vector2.zero;
        controls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => moveInput = Vector2.zero;
        controls.Character.Run.performed += context =>
        {
            speed = runSpeed;
            isRunning = true;
        };
        controls.Character.Run.canceled += context =>
        {
            speed = walkSpeed;
            isRunning = false;
        };
    }

        

  
}
