using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private bool isCrouching = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) 
        {
            ApplyCrouch();
        }
    }

    private void ApplyCrouch()
    {
        isCrouching = !isCrouching;
        animator.SetBool("IsCrouching", isCrouching);     

        if (isCrouching)
        {
            characterController.height = crouchHeight;
        }
        else
        {
            characterController.height = 1.8f;   
            characterController.center = new Vector3(0,0.93f,0);
            animator.SetBool("CrouchToStand", true);
        }
    }
}
