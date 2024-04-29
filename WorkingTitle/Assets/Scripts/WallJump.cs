using System.Collections;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    [SerializeField] private float OverlapSphereRadius = 3.0f;
    [SerializeField] private bool PlayerCanWallJump = false;
    [SerializeField] private Transform RaycastTransform;
    [SerializeField] private float RaycastDistance = 1.0f;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private bool PlayerOnRightWall = false;
    [SerializeField] private float gravityTime = 2.0f;
    [SerializeField] private float timer = 0.0f;
    [SerializeField] private bool applyGravity = false;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool checkingTimer = false;
    [SerializeField] private Animator anim;
    private RaycastHit rightRay;
    private RaycastHit leftRay;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, OverlapSphereRadius);
        foreach (Collider colliders in collider)
        {
            if (colliders.tag == "WallJump")
            {
                Debug.Log("player can wallJump");
                PlayerCanWallJump = true;
                break;
            }
            else
            {
                PlayerCanWallJump = false;
            }
        }

        if (PlayerCanWallJump)
        {
            WallJumpImplementation();

            if (isGrounded)
            {
                rb.useGravity = false;
                
            }
        }

        if (checkingTimer)
        {
            CheckTimer();
        }
    }

    private void WallJumpImplementation()
    {
        bool hitRightWall = Physics.Raycast(RaycastTransform.position, transform.right, out rightRay, RaycastDistance);
        bool hitLeftWall = Physics.Raycast(RaycastTransform.position, -transform.right, out leftRay, RaycastDistance);

        if (hitRightWall && !PlayerOnRightWall)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Right wall jump");
                characterController.enabled = false;
                Vector3 destination = rightRay.point;
                WallJumpRay(destination);
                PlayerOnRightWall = true;
            }
        }
        else if ((hitLeftWall && PlayerOnRightWall && !isGrounded))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Left wall jump");
                characterController.enabled = false;
                Vector3 destination = leftRay.point;
                WallJumpRay(destination);
                PlayerOnRightWall= false;
            }
        }
        else if(!hitLeftWall && !hitRightWall && !isGrounded)
        {
            PlayerOnRightWall = false;
            anim.SetLayerWeight(1, 0);
            //characterController.enabled = true;
            Debug.Log("PLAYER RECHED");
        }
        else if(hitLeftWall && hitRightWall && isGrounded)
        {
            PlayerOnRightWall = false;
            anim.SetLayerWeight(1, 0);
        }
        else if (!hitLeftWall && !hitRightWall && !isGrounded)
        {
            Debug.Log("Player unable to reach top postion");
        }
    }


    private void CheckTimer()
    {
        timer += Time.deltaTime;       
        if (timer >= gravityTime)   //checking if timer is greater or equal to the mentioned time [2 seconds in this case]
        {
            rb.useGravity = true;
            characterController.enabled = true;
            anim.SetTrigger("Idle");
            anim.SetLayerWeight(1, 0);
            timer = 0.0f; 
            checkingTimer = false;
        }
    }

    private void WallJumpRay(Vector3 destination)
    {
       transform.position =  Vector3.Lerp(transform.position, destination,1.0f);
       StartCoroutine(StartAnimation());
        checkingTimer = true;
        timer = 0.0f;
    }

    private void ResetTime()
    {
        timer = 0.0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, OverlapSphereRadius);
        Gizmos.DrawRay(RaycastTransform.position, transform.right * RaycastDistance);
        Gizmos.DrawRay(RaycastTransform.position, -transform.right * RaycastDistance);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    IEnumerator StartAnimation()
    {
        anim.SetLayerWeight(1, 2);         
        anim.SetTrigger("WallJump");        // wall jump aniamtiion
        anim.SetBool("Jump", false); ;
        yield return new WaitForSeconds(0.40f);
        anim.SetTrigger("WallJumpIdle");        // for sliding animation 
    }
}
