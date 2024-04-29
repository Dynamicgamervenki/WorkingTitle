using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform WallCheck;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private bool isWallSliding = false;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallSlidingSpeed = 2.0f;
    [SerializeField] private float radius = 1.0f;
    [SerializeField] private bool isGrounded;

    private void Awake()
    {
          rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        WallSlide();
    }

    private bool is_Grounded()
    {
        return Physics.CheckSphere(GroundCheck.position, radius, groundLayer);
    }

    private bool isWalled()
    {
        if (Physics.CheckSphere(WallCheck.position, radius, wallLayer))
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            return true;
        }
        return false;
    }


    private void WallSlide()
    {
        if(isWalled() && !is_Grounded())
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

        if (is_Grounded())
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(WallCheck.position, radius);
        Gizmos.DrawWireSphere(GroundCheck.position, radius);
    }
}
