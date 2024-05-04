using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    PlayerLocomotion playerLocomotion;

    [Header("Inputs")]
    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    private float moveAmount;
    public float horizontalInput;
    public float verticalInput;

    public bool jumpInput;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += ctx =>
            {
                movementInput = ctx.ReadValue<Vector2>();
            };

            playerControls.PlayerMovement.Camera.performed += ctx =>
            {
                cameraInput = ctx.ReadValue<Vector2>();
            };

            playerControls.PlayerMovement.Jump.performed += ctx =>
            {
                jumpInput = true;
            };
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        // for calling all the functions , this function itself is called in Update function Nigga
        HandleMovementInput();
        HandleJumpingInput();
    }

    private void HandleMovementInput()
    {
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0,moveAmount);
    }

    private void HandleJumpingInput()
    {
        if (jumpInput)
        {
            jumpInput = false;
            playerLocomotion.HandleJumping();
        }
    }
}
