using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Mechanics : MonoBehaviour
{
    [HideInInspector]public Animator anim;
    NewRootMotionController motionController;
   public  CharacterController characterController;

    public float rotationSpeed;
    Quaternion targetRotation;
    public bool inControl;
    bool ropeToWallClimbing = false;


    public bool isBalanceWalking = false;

    [Header("Rope Climbing")]
    public float climbSpeed = 0.75f;
    public bool withinRopeRadius = false;
    public bool isRopeClimbing = false;
    public float yOffset;
    public float ropeDetectionRadius = 2.0f;
    public float maxDistance = 2.0f;
    public LayerMask ropeMask;

    public Transform shell;
    public bool waitFor2Sec = false;


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
        if (isRopeClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 climbMovement = new Vector3(0, verticalInput * climbSpeed * Time.deltaTime, 0);

            if (canClimbEdge/* && verticalInput > 0f*/)
            {
                motionController.isJumping = false;
            }
            //  characterController.Move(climbMovement);


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
        this.inControl = inControl;
        characterController.enabled = inControl;

        if (!inControl)
        {
            anim.SetFloat("Locomotion", 0f);
            targetRotation = transform.rotation;
        }
    }

    bool hasExectuted = false;

    private void RopeClimbing()
    {
        withinRopeRadius = Physics.SphereCast(transform.position + Vector3.up * yOffset, ropeDetectionRadius, transform.forward, out RaycastHit hit, maxDistance, ropeMask);


        if (withinRopeRadius && motionController.isJumping)
        {
            isRopeClimbing = true;
            anim.Play("Rope", 0);
            characterController.enabled = false;
            transform.position = hit.point;
            characterController.enabled = true;
            if (!hasExectuted)
                this.transform.SetParent(hit.transform);
            hit.transform.Rotate(0f, 180f, 0f);
            this.transform.SetParent(null);
            hasExectuted = true;
        }


    }




    public bool isCrouched = false;
    public GameObject crouchObj;
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
            crouchObj.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }
        if(!isCrouched)
        {
            Debug.Log("stand up");
            anim.SetBool("isCrouched", false);
            crouchObj.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
    }



    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * yOffset + transform.forward * maxDistance, ropeDetectionRadius);


        //Vector3 origin = transform.position + Vector3.up * yOffset;
        //Vector3 direction = transform.forward;

        //// Perform the SphereCast
        //bool withinRopeRadius = Physics.SphereCast(origin, ropeDetectionRadius, direction, out RaycastHit hit, maxDistance, ropeMask);

        //// Draw the sphere at the origin of the SphereCast
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(origin, ropeDetectionRadius);

        //// Draw a line in the direction of the SphereCast
        //Gizmos.color = Color.green;
        //Gizmos.DrawLine(origin, origin + direction * maxDistance);

        //// If a hit is detected, draw the hit point
        //if (withinRopeRadius)
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(hit.point, ropeDetectionRadius);
        //}

    }


}
