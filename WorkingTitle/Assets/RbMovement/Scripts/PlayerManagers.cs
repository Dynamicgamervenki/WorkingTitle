using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagers : MonoBehaviour
{
    Animator anim;
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    CameraManager cameraManager;

    public bool isInteracting;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        cameraManager = FindObjectOfType<CameraManager>();
        anim = GetComponent<Animator>();

    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovements();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovemnts();

        isInteracting = anim.GetBool("isInteracting");
        playerLocomotion.isJumping = anim.GetBool("isJumping");
        anim.SetBool("isGrounded", playerLocomotion.isGrounded);
        anim.SetBool("isWallSliding",playerLocomotion.is_wallSliding);
    }
}
