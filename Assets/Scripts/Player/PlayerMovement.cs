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

    private void Start() {
        characterController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();
        speed = walkSpeed;
        AssignInputEvents();
        
    }
    private void Update()
    {
        if(player.health.isDead)
            return;
        ApplyMovement();
        ApplyRotation();
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
        if (movementDir.magnitude > 0)
        {
            characterController.Move(movementDir * Time.deltaTime * speed);
        }
    }

    private void ApplyGavility()
    {
        // if Player is ground Player will reduce position y
        if(characterController.isGrounded == false)
        {
            verticalVeloccity = verticalVeloccity - 9.81f * Time.deltaTime;
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
