using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mechanics : MonoBehaviour
{
    [HideInInspector]public Animator anim;
    NewRootMotionController motionController;
    CharacterController characterController;

    [Header("Ground Check")]
    public bool isGrounded = true;
    public Vector3 groundCheckOffset;
    public LayerMask groundMask;
    public float groundCheckRadius = 0.2f;

    public float rotationSpeed;
    Quaternion targetRotation;
    bool hasControl;
    bool ropeToWallClimbing = false;

    [Header("Interaction")]
    public LayerMask interaction;
    public bool isInteracting = false;

    public bool isBalanceWalking = false;

    [Header("Rope Climbing")]
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
        RopeClimbing();
        Crouch();

        if (isRopeClimbing)
        {
            anim.SetBool("isRopeClimbing", true);
            if(Input.GetAxis("Vertical") > 0f)
            {
                anim.SetFloat("moveY", 1.0f);
            }
            else if(Input.GetAxis("Vertical") == 0.0f)
            {
                anim.SetFloat("moveY", 0.0f);
            }
            else if (Input.GetAxis("Vertical") < 0.0f)
            {
                anim.SetFloat("moveY", 2.0f);
            }
        }

        Debug.Log("isGrounded : " + isGrounded);
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

    public void OnControl(bool inControl)
    {
        this.hasControl = inControl;
        characterController.enabled = inControl;

        if(!hasControl)
        {
            anim.SetFloat("moveAmount", 0.0f);

        }

    }

    private void RopeClimbing()
    {
        if(Physics.SphereCast(transform.position + Vector3.up * yOffset,ropeDetectionRadius,transform.forward,out RaycastHit hit,maxDistance,ropeMask))
        {
            if (hit.transform.gameObject.layer == ropeMask)
                Debug.Log("RopeFound");
        }
    }

    bool isCrouched = false;

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

    private void Interaction()      //for pushing objects
    {
        if(Physics.Raycast(transform.position + Vector3.up * 0.2f,transform.forward,0.2f,interaction))
        {
            isInteracting = true;
        }
        else
        {
            isInteracting = false;
        }
        anim.SetBool("isInteracting", isInteracting);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Vector4(0, 1, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);

        Gizmos.color = Color.yellow;
        Debug.DrawRay(transform.position +  Vector3.up * 0.3f, transform.forward * 0.2f);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * yOffset + transform.forward * maxDistance,ropeDetectionRadius);
    }


}
