using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    PlayerManagers playerManagers;
    AnimatorManager animatorManager;

    private Vector3 moveDirection;
    private Transform camera;
    private Rigidbody rb;


    [Header("Movemnt")]
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float rotationSpeed = 10.0f;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public LayerMask groundLayer;
    public float rayCastOffSet = 0.5f;

    public bool isGrounded;
    public bool isSprinting;

    [Header("Jumping")]
    public bool isJumping;
    public float gravityIntensity = -15f;
    public float jumpHeight = 5.0f;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerManagers = GetComponent<PlayerManagers>();
        animatorManager = GetComponent<AnimatorManager>();  

        rb = GetComponent<Rigidbody>();
        camera = Camera.main.transform;
    }

    public void HandleAllMovements()    //reagrding rigidbody ,will be called in fixed update
    {
        HandleFallingAndLanding();

        if (playerManagers.isInteracting)
            return;

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        if (isJumping)
            return;


        moveDirection = camera.forward * inputManager.verticalInput;
        moveDirection = moveDirection + camera.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        if(isJumping)
            return ;

        Vector3 targetDirection = Vector3.zero;

        targetDirection = camera.forward * inputManager.verticalInput;
        targetDirection = targetDirection + camera.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation,targetRotation,rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 RaycastOrigin = transform.position;
        RaycastOrigin.y = RaycastOrigin.y + rayCastOffSet;

        if(!isGrounded && !isJumping)
        {
            if(!playerManagers.isInteracting)
            {
                animatorManager.PlayTargetAnimations("Falling", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if(Physics.SphereCast(RaycastOrigin,0.2f,-Vector3.up,out hit,groundLayer))
        {
            if(!isGrounded && !playerManagers.isInteracting)
            {
                animatorManager.PlayTargetAnimations("Land",true);
            }
            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded=false;
        }

    }

    public int jumpsPerformed = 0;
    public void HandleJumping()
    {
        if ((isGrounded || !isGrounded && jumpsPerformed <2))
        {
            jumpsPerformed++;

            animatorManager.anim.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimations("Jump", true);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            rb.velocity = playerVelocity;
            
            if(jumpsPerformed == 2)
            {
                jumpsPerformed = 0;
            }
        }
    }

}
