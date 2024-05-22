using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.Profiling;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    PlayerManagers playerManagers;
    AnimatorManager animatorManager;

    private Vector3 moveDirection;
    private Transform camera;
    private Rigidbody rb;


    [Header("Movemnt")]
    [SerializeField] private float RunningSpeed = 5.0f;
    [SerializeField] private float walkingSpeed = 1.5f;
    [SerializeField] private float sprintingSpeed = 7.0f;
    [SerializeField] private float rotationSpeed = 10.0f;
    public bool is_sprinting;

    public bool player_Moving;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public LayerMask groundLayer;
    public float sphereRadius = 0.2f;
    public float rayCastOffSet = 0.5f;
    public bool isGrounded;

    [Header("Jumping")]
    public bool isJumping;
    public float gravityIntensity = -15f;
    public float jumpHeight = 5.0f;
    public int jumpsPerformed = 0;

    [Header("PushAndPull")]
    public LayerMask interactionMask;
    public bool isPushingOrPulling = false;
    public Transform interactionSphereCast;
    GameObject hitObject;
    public float pushSpeed;

    [Header("Rope Climbing")]
    public Transform SphereCastRope;
    public float ropeDetectionRaidus = 1.0f;
    public float ropeClimbingSpeed = 0.5f;
    public LayerMask ropeMask;
    public LayerMask groundMask;
    public bool rope_climbing = false;
    public float maxDistance = 0.5f;
    public bool playerTryingToGround;
    public bool rope_swinging = false;
    public GameObject RopeRigid;

    [Header("Wall Jump")]
    public LayerMask wallMask;
    public bool WallDetected;
    public float WallMaxDistance;
    public float yOffset = 0.3f;
    public float wallDetectRadius = 0.2f;
    public bool is_wallSliding;
    private RaycastHit WallHit;
    public float slidingVelocity = 2.0f;
    public float inAirTime;


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
        HandleWallJumpDetection();
        inputManager.HandleWallJumpInput();
        HandleRopeClimbing();
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

        if (isPushingOrPulling)
        {
            moveDirection = camera.forward * Mathf.Clamp(inputManager.verticalInput, 0f, 1f);
            moveDirection.Normalize();
            moveDirection *= pushSpeed;

            if (inputManager.verticalInput < 0 || inputManager.horizontalInput != 0)
            {
                isPushingOrPulling = false;
                hitObject.transform.SetParent(null);
                animatorManager.anim.SetBool("isPushing", false);
            }
        }
        else if(rope_climbing)
        {
            moveDirection = transform.up * inputManager.verticalInput;
            moveDirection.Normalize();
            moveDirection *= ropeClimbingSpeed;              
        }
        else
        {
            moveDirection = camera.forward * inputManager.verticalInput;
            moveDirection += camera.right * inputManager.horizontalInput;
            moveDirection.Normalize();
            moveDirection.y = 0f;

            if(is_sprinting && !rope_climbing)
            {
                moveDirection *= sprintingSpeed;
            }
            else
            {
                if (inputManager.moveAmount >= 0.5f)
                    moveDirection *= RunningSpeed;
                else
                    moveDirection *= walkingSpeed;
            }
        }

        player_Moving = moveDirection.magnitude > 0;

        rb.velocity = moveDirection;
    }

    private void HandleRotation()
    {
        if(isJumping)
            return ;

        if(rope_climbing)
            return;

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
        if (rope_climbing)
            return;

        RaycastHit hit;
        Vector3 RaycastOrigin = transform.position;
        RaycastOrigin.y = RaycastOrigin.y + rayCastOffSet;

        if (!isGrounded && !isJumping && !WallDetected)
        {
            if (!playerManagers.isInteracting || !rope_climbing /*|| !WallDetected*/)
            {
                animatorManager.PlayTargetAnimations("Falling", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(RaycastOrigin, sphereRadius,Vector3.down, out hit,groundLayer))
        {
            if (!isGrounded && !playerManagers.isInteracting)
            {
                animatorManager.PlayTargetAnimations("Land", true);
            }
            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }


    public void HandleJumping()
    {
        if (WallDetected)
            return;

        if(rope_climbing)
            return;

        if (isGrounded || jumpsPerformed < 2)
        {
            if (isGrounded)
            {
                jumpsPerformed = 0;
            }

            jumpsPerformed++;

            animatorManager.anim.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimations("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            rb.velocity = playerVelocity;
        }

    }



    public void HandlePushAndPull()
    {
        RaycastHit hit;
        interactionSphereCast.position = new Vector3(interactionSphereCast.position.x, 0.5f, interactionSphereCast.position.z);

        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f, interactionMask))
        {
             hitObject = hit.transform.gameObject;
            if (!isPushingOrPulling)
            {
                    isPushingOrPulling = true;
                    hitObject = hit.transform.gameObject;
                    hitObject.transform.SetParent(transform);
                    animatorManager.anim.SetBool("isPushing", true);
            }
            else if(isPushingOrPulling)
            {
                Debug.Log("TRYING TO UNPARENT");
                isPushingOrPulling = false;
                hitObject.transform.SetParent(null);
                animatorManager.anim.SetBool("isPushing", false);
            }
        }
        else
        {
            return;
        } 
    }

    public void HandleRopeClimbing()
    {
        if (Physics.SphereCast(transform.position + Vector3.up * 0.3f, ropeDetectionRaidus, transform.forward, out RaycastHit hit, maxDistance, ropeMask))
        {
            Debug.Log("rope found !"); isGrounded = true;
            rope_climbing = true;
            transform.SetParent(hit.transform);
            animatorManager.anim.SetBool("isRopeClimbing", true);
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
        }
        else
        {
            rope_climbing = false;
            transform.SetParent(null);
            animatorManager.anim.SetBool("isRopeClimbing", false);
           // rb.useGravity = true;
        }

        if ( rope_climbing && Physics.Raycast(transform.position, -transform.up, out RaycastHit hit1, 0.5f, groundMask))
        {
            playerTryingToGround = true;
        }
        else
        {
            playerTryingToGround = false;
        }

        if (rope_climbing && playerTryingToGround && inputManager.verticalInput < 0)
        {
            rope_climbing = false;
            transform.SetParent(null);
            animatorManager.PlayTargetAnimations("Falling", true);
            rb.useGravity = true;
        }
    }


    public void HandleWallJumpDetection()
    {
        WallDetected = Physics.Raycast(transform.position + Vector3.up * yOffset, transform.forward, out WallHit, WallMaxDistance, wallMask);   
        if (is_wallSliding)
        {
            inAirTime = 0;
            inAirTime = inAirTime + Time.time;

            rb.AddForce(Vector3.down * inAirTime * slidingVelocity);
        }
        else
        {
            inAirTime = 0;
        }

        if(!WallDetected)
        {
            rb.useGravity = true;
        }

    }

    public void HandleWallJump()
    {
        if (WallDetected)
        {
            rb.useGravity = false;
            RotatePlayer(180);
            transform.position = Vector3.Lerp(transform.position, WallHit.point, 1.0f);
            animatorManager.PlayTargetAnimations("WallJump", false);
        }
    }

    void RotatePlayer(float angle)
    {
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + angle, transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1.0f);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        interactionSphereCast.position = new Vector3(interactionSphereCast.position.x,0.5f,interactionSphereCast.position.z);
        Gizmos.DrawRay(interactionSphereCast.transform.position,transform.forward * 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.3f + transform.forward * maxDistance,ropeDetectionRaidus);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position,-transform.up * 0.5f);

        Gizmos.color = Color.yellow;
       // Gizmos.DrawWireSphere(transform.position + Vector3.up * yOffset + transform.forward * WallMaxDistance,wallDetectRadius);

        Gizmos.DrawRay(transform.position + Vector3.up * yOffset, transform.forward * WallMaxDistance);
        //Gizmos.color = Color.red;
        //Gizmos.DrawRay(transform.position + Vector3.up * yOffset, -Vector3.right * WallMaxDistance);
    }




}
