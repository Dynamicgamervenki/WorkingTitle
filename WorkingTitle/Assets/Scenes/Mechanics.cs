using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mechanics : MonoBehaviour
{
    public Animator anim;
    public bool isRopeClimbing = false;  
    RootMotionController rootMotionController;
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

    private void Awake()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
         rootMotionController = GetComponent<RootMotionController>();
    }

    private void Update()
    {
        GroundCheck();
        isGrounded = gameObject.GetComponent<CharacterController>().isGrounded;

        if(isRopeClimbing)
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
        if (hit.gameObject.CompareTag("Rope"))
        {
                Debug.Log("In Contact With Rope");
                isRopeClimbing = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "climb" && isRopeClimbing)
        { 
            ropeToWallClimbing = true;
        }
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundMask);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Vector4(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }


}
