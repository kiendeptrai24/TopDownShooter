using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;
    private CharacterController characterController;
    private Animator anim;

    [Header("Movement info")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSpeed;
    private float speed;
    public Vector3 movementDir;
    private float verticalVeloccity;
    public Vector2 moveInput { get; private set; }

    private bool isRunning;
    private AudioSource walkSFX;
    private AudioSource runSFX;
    private bool canPlayFootsteps;

    // ///////////
    public float distance = 3.0f; // Khoảng cách camera
    public float mouseSensitivity = 100f;
    public float minY = -20f, maxY = 60f; // Giới hạn góc nhìn

    private float currentX = 0f;
    private float currentY = 0f;
    ///

    private void Start() {
        characterController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();
        speed = walkSpeed;
        walkSFX = player.sound.walkSFX;
        runSFX = player.sound.RunSFX;
        Invoke(nameof(AllowFootstepsSFX),1f);
        AssignInputEvents();
        movementDir = Vector3.zero;
        
    }
    private void Update()
    {
        if (player.health.isDead)
            return;
        ApplyMovement();
        ApplyRotation();
        AnimationControllers();
        // ApplyMouse();
        // LateUpdate();
    }

    private void ApplyMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        currentX += mouseX;
        currentY -= mouseY;
        //currentY = Mathf.Clamp(currentY, minY, maxY);
    }
    void LateUpdate()
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        CameraManager.Instance.transform.position = transform.position + rotation * direction;
        CameraManager.Instance.transform.LookAt(transform.position);
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

    private void ApplyRotation()
    {
        //getting mouse's direction
        Vector3 lookingDir = player.aim.GetMouseHitInfo().point - transform.position;
        lookingDir.y = 0;

        //Quaternion direction and Slerp direction between player ad aim 
        Quaternion DesiredRotation = Quaternion.LookRotation(lookingDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, DesiredRotation,turnSpeed * Time.deltaTime);

        
    }

    private void ApplyMovement()
    {
        movementDir = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGavility();
        if (movementDir.magnitude > 0 && characterController.enabled)
        {
            Vector3 move = new Vector3(movementDir.x,0,movementDir.z);

            if(move.magnitude > .001f)
                PlayFootstepsSFX();
            
            
            characterController.Move(movementDir * Time.deltaTime * speed);

        }
    }

    private void PlayFootstepsSFX()
    {
        if(canPlayFootsteps == false)
            return;
        if (isRunning)
        {
            if (runSFX.isPlaying == false)
            {
                walkSFX.Stop();
                runSFX.Play();
            }
        }
        else
        {
            if (walkSFX.isPlaying == false)
            {
                runSFX.Stop();
                walkSFX.Play();
            }
        }
    }
    private void AllowFootstepsSFX() => canPlayFootsteps = true;

    private void StopFootstepsSFX()
    {
        walkSFX.Stop();
        runSFX.Stop();
    }

    private void ApplyGavility()
    {
        // if Player is ground Player will reduce position y
        if(characterController.isGrounded == false)
        {
            verticalVeloccity = verticalVeloccity - 9.81f * Time.deltaTime;
            movementDir.y = verticalVeloccity;
        }
        else if(verticalVeloccity != -.5f)
            verticalVeloccity = -.5f;    
        
    }
    
    private void AssignInputEvents()
    {
        controls = player.controls;
        controls.Character.Movement.performed += context =>
        {
            moveInput = context.ReadValue<Vector2>();
        };
        controls.Character.Movement.canceled += context =>
        {
            StopFootstepsSFX();
            moveInput = Vector2.zero;
        };

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
