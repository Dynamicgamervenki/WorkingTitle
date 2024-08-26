using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Mechanics : MonoBehaviour
{
    [HideInInspector]public Animator anim;
    NewRootMotionController motionController;
    CharacterController characterController;

    public float rotationSpeed;
    Quaternion targetRotation;
    public bool inControl;
    bool ropeToWallClimbing = false;


    public bool isBalanceWalking = false;

    [Header("Rope Climbing")]
    float climbSpeed = 0.75f;
    public bool withinRopeRadius = false;
    public bool isRopeClimbing = false;
    public float yOffset;
    public float ropeDetectionRadius = 2.0f;
    public float maxDistance = 2.0f;
    public LayerMask ropeMask;



    private void Awake()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        motionController = FindObjectOfType<NewRootMotionController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Crouch();
        //PerformClimbingParkourAction();
        if (isRopeClimbing)
        {

            float verticalInput = Input.GetAxis("Vertical");

            Vector3 climbMovement = new Vector3(0, verticalInput * climbSpeed * Time.deltaTime, 0);

            if (canClimbEdge && verticalInput > 0f)
            {
                climbMovement = Vector3.zero;
            }

            characterController.Move(climbMovement);

            if (verticalInput > 0f)
            {
                Debug.Log("Rope climbing up");
                anim.SetFloat("moveY", 1.0f); // Climbing up
            }
            else if (verticalInput == 0.0f)
            {
                Debug.Log("Rope climb idle");
                anim.SetFloat("moveY", 0.0f); // Idle
            }
            else if (verticalInput < 0.0f)
            {
                Debug.Log("Rope climbing down");
                anim.SetFloat("moveY", 2.0f); // Climbing down
            } 
            
        }

    }

    private void FixedUpdate()
    {
        RopeClimbing();
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.CompareTag("Balance"))
        {
            Debug.Log("balance started");
            isBalanceWalking = true;
            anim.SetLayerWeight(2, 1.0f);
        }
        else
        {
            isBalanceWalking = false;
            anim.SetLayerWeight(2, 0.0f);
        }
    }

    public bool canClimbEdge = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("climb"))
            canClimbEdge = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("climb"))
            canClimbEdge = false;
    }

    public void OnControl(bool inControl)
    {
        //motionController.enabled = inControl;
        //anim.applyRootMotion = inControl;
        this.inControl = inControl;
        characterController.enabled = inControl;

        if (!inControl)
        {
            anim.SetFloat("moveAmount", 0f);
            targetRotation = transform.rotation;
        }
    }

    private void RopeClimbing()
    {
        withinRopeRadius = Physics.SphereCast(transform.position + Vector3.up * yOffset, ropeDetectionRadius, transform.forward, out RaycastHit hit, maxDistance, ropeMask);


        if(withinRopeRadius && motionController.isJumping)
        {
            isRopeClimbing = true;
            anim.Play("Traversal_Pole_Climb_Enter", 0);
            characterController.enabled = false;
            transform.position = hit.point;
            characterController.enabled = true;
        }

        //if (!withinRopeRadius && isRopeClimbing)
        //{
        //    isRopeClimbing = false;
        //    anim.SetBool("isRopeClimbing", false);
        //}



    }

    //public bool inAction = false;
    //private void PerformClimbingParkourAction()
    //{
    //    if (canClimbEdge)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            anim.applyRootMotion = true;
    //            characterController.enabled = false;
    //            anim.CrossFade("StepUp", 0.2f);
    //            inAction = true;
    //            isRopeClimbing = false;
    //            Invoke("EnableChracterController", 1.350f);
    //        }
    //    }
    //}

    //private void EnableChracterController()
    //{
    //    inAction = false;
    //    characterController.enabled = true;
    //}


    public bool isCrouched = false;

    private void Crouch()
    {
        if (motionController.Grounded && Input.GetKeyDown(KeyCode.C))
        {
            isCrouched = !isCrouched;
        }

        if(isCrouched)
        {
            Debug.Log("Crouched");
            anim.SetBool("isCrouched", true);
        }
        if(!isCrouched)
        {
            Debug.Log("stand up");
            anim.SetBool("isCrouched", false);
        }
    }



    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = new Vector4(0, 1, 0, 0.5f);
        //Gizmos.DrawWireSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);

        //Gizmos.color = Color.yellow;
        //Debug.DrawRay(transform.position +  Vector3.up * 0.3f, transform.forward * 0.2f);

        //Gizmos.color = Color.black;
        //Gizmos.DrawWireSphere(transform.position + Vector3.up * yOffset + transform.forward * maxDistance,ropeDetectionRadius);
        Vector3 origin = transform.position + Vector3.up * yOffset;
        Vector3 direction = transform.forward;

        // Perform the SphereCast
        bool withinRopeRadius = Physics.SphereCast(origin, ropeDetectionRadius, direction, out RaycastHit hit, maxDistance, ropeMask);

        // Draw the sphere at the origin of the SphereCast
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(origin, ropeDetectionRadius);

        // Draw a line in the direction of the SphereCast
        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + direction * maxDistance);

        // If a hit is detected, draw the hit point
        if (withinRopeRadius)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hit.point, ropeDetectionRadius);
        }

    }


}
