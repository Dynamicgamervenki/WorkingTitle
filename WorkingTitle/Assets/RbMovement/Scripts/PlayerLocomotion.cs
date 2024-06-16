using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net;

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
    public Rigidbody rb;


    [Header("Movemnt")]
    [SerializeField] private float RunningSpeed = 5.0f;
    [SerializeField] private float walkingSpeed = 1.5f;
    [SerializeField] private float sprintingSpeed = 7.0f;
    [SerializeField] private float rotationSpeed = 10.0f;
    public LayerMask playerMask;
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
    public Transform GroundCheck;
    Vector3 RaycastOrigin;

    [Header("Jumping")]
    public bool isJumping;
    public float gravityIntensity = -15f;
    public float jumpHeight = 5.0f;
    public int jumpsPerformed = 0;

    [Header("PushAndPull")]
    public LayerMask PullAndPushMask;
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
    public bool climbingCliff =false;
    public Transform ClimbPoint;

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
    public LayerMask swingMask;
    public float swingRadius;
    public bool swing;

    [Header("Crouching")]
    public bool isPlayerCrouching;
    public float crouchWalkSpeed = 1.0f;

    private bool ClimbingOnScale = false;


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

        HandleWallJumpDetection();
        inputManager.HandleWallJumpInput();
        inputManager.HandlePullAndPushInputs();
        HandleRopeSwing();
        HandleRopeClimbing();


        if (playerManagers.isInteracting)
            return;

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        if (isJumping)
            return;

        if(swing)
            return;


        if (isPushingOrPulling)
        {
            moveDirection = camera.forward * Mathf.Clamp(inputManager.verticalInput, 0f, 1f);
            moveDirection.Normalize();
            moveDirection *= pushSpeed;

            if (inputManager.verticalInput < 0 || inputManager.horizontalInput != 0)
            {
                isPushingOrPulling = false;
               // hitObject.transform.SetParent(null);
                animatorManager.anim.SetBool("isPushing", false);
            }
        }
        else if(rope_climbing)
        {
            moveDirection = transform.up * inputManager.verticalInput;
            moveDirection.Normalize();
            moveDirection *= ropeClimbingSpeed;              
        }
        //else if(climbingCliff)
        //{
        //    moveDirection = transform.up * inputManager.verticalInput;
        //    moveDirection.Normalize();
        //    moveDirection *= 10.0f;
        //}
        else
        {
            moveDirection = camera.forward * inputManager.verticalInput;
            moveDirection += camera.right * inputManager.horizontalInput;
            moveDirection.Normalize();
            moveDirection.y = 0f;

            if(is_sprinting && !rope_climbing && !isPlayerCrouching)
            {
                moveDirection *= sprintingSpeed;
            }
            else if(isPlayerCrouching)
            {
                moveDirection *= crouchWalkSpeed;
            }
            else if(animatorManager.Balancing)
            {
                moveDirection *= 0.5f;
            }
            else if(ClimbingOnScale)
            {
                moveDirection *= 1.1f;
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

        if(swing)
            return ;

        if (isPlayerCrouching)
            return;

        if(isPushingOrPulling)
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
        if (rope_climbing)
            return;

        if (swing)
            return;

        if (isPlayerCrouching)
            return;

        //if (climbingCliff)
        //    return ;


        RaycastHit hit;
        RaycastOrigin = transform.position;
        RaycastOrigin.y = RaycastOrigin.y + rayCastOffSet;

        if (!isGrounded && !isJumping && !WallDetected)
        {
            if (!playerManagers.isInteracting /*|| !rope_climbing*/)
            {
                    animatorManager.PlayTargetAnimations("Falling", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(GroundCheck.position, sphereRadius,-Vector3.up, out hit,1.0f,groundLayer))
        {
            if (!isGrounded && !playerManagers.isInteracting)
            {
                      animatorManager.PlayTargetAnimations("Landing", true);
            }
           // if(swing)
           //  isGrounded = false;
            inAirTimer = 0;
            isGrounded = true; 
            rb.useGravity = true;  /*climbingCliff = false;*/
        }
        else
        {
            isGrounded = false;
        }


    }

    public void HandleEvent()
    {
        Debug.Log("reached !");
        ClimbPoint.transform.GetComponentInParent<BoxCollider>().isTrigger = false;
   //     climbingCliff = false;
        animatorManager.anim.SetBool("Climbing",false);
        Quaternion targetRotation = Quaternion.Euler(0, -0, -0);
        transform.position = Vector3.Lerp(transform.position,ClimbPoint.transform.position,1.0f * Time.deltaTime);
        //Time.timeScale = 0;

        // transform.position = Vector3.Lerp(transform.position, ClimbPoint.position, 1.0f);

        //    Time.timeScale = 0;
        //transform.position = new Vector3(0.674892f,transform.position.y,transform.position.z);
        //   animatorManager.anim.SetBool("Climbing",false);
        //transform.position += transform.up + transform.forward * 0.5f;
    }
    public void HandleJumping()
    {
        if (WallDetected)
            return;

        if(rope_climbing)
            return;

        if(swing && !rope_swinging)
            return;

        if (isPlayerCrouching)
            return;


        if (swing)
        {
            animatorManager.anim.SetBool("isRopeSwinging", false);
            swing = false;
            swingRadius = 0f;
            transform.SetParent(null);
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            

            rb.AddForce(transform.forward * 5.0f, ForceMode.Impulse);
            Invoke("DetectCollisions", 1.0f);

            this.transform.rotation = Quaternion.Euler(0,180, 0);

            //    rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);

        }

        if (isGrounded || jumpsPerformed < 2)
        {
            if (isGrounded)
            {
                jumpsPerformed = 0;
            }

            jumpsPerformed++;

            animatorManager.anim.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimations("Jump", true);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            rb.velocity = playerVelocity;
        }

        //if(isGrounded)                        // for single jump.
        //{
        //    animatorManager.anim.SetBool("isJumping", true); 
        //    animatorManager.PlayTargetAnimations("Jump", false);

        //    float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
        //    Vector3 playerVelocity = moveDirection;
        //    playerVelocity.y = jumpingVelocity;
        //    rb.velocity = playerVelocity;

        //}

    }



    public void HandlePushAndPull()
    {
        RaycastHit hit;
       // interactionSphereCast.position = new Vector3(interactionSphereCast.position.x, 0.5f, interactionSphereCast.position.z);

        if (Physics.Raycast(transform.position + Vector3.up * 0.3f, transform.forward, out hit, 0.5f, PullAndPushMask))
        {
            // hitObject = hit.transform.gameObject;
            hit.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            hit.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;
            hit.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
            if (!isPushingOrPulling)
            {
                    isPushingOrPulling = true;
                    rb.AddForce(transform.forward * 2.0f, ForceMode.Force);
                   // hitObject = hit.transform.gameObject;
                  //  hitObject.transform.SetParent(transform);
                    animatorManager.anim.SetBool("isPushing", true);
            }
            else if(isPushingOrPulling)
            {
                Debug.Log("TRYING TO UNPARENT");
                isPushingOrPulling = false;
                //hitObject.transform.SetParent(null);
                animatorManager.anim.SetBool("isPushing", false);
            }
        }
        else
        {
            //animatorManager.anim.SetBool("isPushing", false);
            return;
        } 
    }

    public void HandleRopeClimbing()
    {
        if (isGrounded)
            return;

        if (Physics.SphereCast(transform.position + Vector3.up * 0.3f, ropeDetectionRaidus, transform.forward, out RaycastHit hit, maxDistance, ropeMask))
        {
            rope_climbing = true;
            animatorManager.anim.SetBool("isRopeClimbing", true);
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
        }
        else
        {
            rope_climbing = false;
            animatorManager.anim.SetBool("isRopeClimbing", false);
        }
    }

    public void HandleRopeSwing()
    {

        Collider[] colliders = (Physics.OverlapSphere(RopeRigid.transform.position,swingRadius));

        foreach (Collider collider in colliders)
        {
            if(collider.tag == "Player")
            {
                swing = true;
                rb.interpolation = RigidbodyInterpolation.None;
                rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            }
        }

        if(swing)
        {
            Debug.Log("swing"); 
            rb.useGravity = false;
            rb.isKinematic = true;
             rb.detectCollisions = false;
            transform.SetParent(RopeRigid.transform.GetChild(0).transform.GetChild(0).transform);
            animatorManager.anim.SetBool("isRopeSwinging", true);
            //animatorManager.PlayTargetAnimations("RopeSwing", true);

            if (rope_swinging)
            {
                Debug.Log("init mate");
                RopeRigid.GetComponent<Rigidbody>().AddForce(transform.forward * inputManager.verticalInput * 20f/*,ForceMode.Acceleration*/);
                RopeRigid.GetComponent<Rigidbody>().AddForce(transform.right * inputManager.horizontalInput * 20f/*,ForceMode.Acceleration*/);
            }
        }
        else
        {
            rb.isKinematic = false;
            rb.useGravity = true;
          //  rb.detectCollisions = true;
        }

    }


    public void HandleWallJumpDetection()
    {
        WallDetected = Physics.Raycast(transform.position + Vector3.up * yOffset, transform.forward, out WallHit, WallMaxDistance, wallMask);   
        if (is_wallSliding)
        {
            inAirTime = 0;
            inAirTime = inAirTime + Time.time;               //time.delta time

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
          //  rb.AddForce(new Vector3(150.0f, 0, 50.0f));
            //rb.velocity = new  Vector3(150,0,150);
            animatorManager.PlayTargetAnimations("WallJump", false);
        }
    }

    public void HandleCrouchMovement()
    {
        if(!isPlayerCrouching)
        {
            Debug.Log("crouch");
            isPlayerCrouching = true;   
            animatorManager.anim.SetBool("isCrouching", true);
        }
        else
        {
            Debug.Log("Stand Up Nigga");
            isPlayerCrouching = false;
            animatorManager.anim.SetBool("isCrouching", false);
        }
    }

    void RotatePlayer(float angle)
    {
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + angle, transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1.0f);
    }

    private void DetectCollisions()
    {
        rb.detectCollisions = true;
    }


    private void OnCollisionEnter(Collision collision)   
    {
        if(collision.gameObject.tag == "Scale")
        {
             ClimbingOnScale = true;
        }
    }

    private void OnCollisionExit(Collision collision) 
    {
        if (collision.gameObject.tag == "Scale")
        {
            ClimbingOnScale = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        interactionSphereCast.position = new Vector3(interactionSphereCast.position.x,0.5f,interactionSphereCast.position.z);
        Gizmos.DrawRay(interactionSphereCast.transform.position,transform.forward * 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.3f + transform.forward * maxDistance,ropeDetectionRaidus);

        Gizmos.color = Color.blue;                      //playertobeground
        Gizmos.DrawRay(transform.position,-transform.up * 0.5f);


        Gizmos.DrawRay(transform.position + Vector3.up * yOffset, transform.forward * WallMaxDistance);

        Gizmos.color = Color.black;     //FOR ROPE REGARDING ROPESWING
        Gizmos.DrawWireSphere(RopeRigid.transform.position,swingRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(RaycastOrigin + Vector3.down,sphereRadius);
        Gizmos.DrawRay(transform.position + Vector3.up * 0.3f,transform.forward * 0.5f);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(GroundCheck.position + (-Vector3.up) * 1.0f,sphereRadius);
    }




}
