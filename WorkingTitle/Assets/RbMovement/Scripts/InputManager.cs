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

    public float moveAmount;
    public float horizontalInput;
    public float verticalInput;

    public bool jumpInput;
    public bool interaction_Input;

    public bool sprintInput;
    public bool wallJumpInput;
    public bool RopeSwingInput;
    public bool crouchInput;

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

            playerControls.PlayerActions.Sprint.performed += ctx =>
            {
                sprintInput = true;
            };


            playerControls.PlayerActions.Sprint.canceled += ctx =>
            {
                sprintInput = false;
            };


            playerControls.PlayerMovement.Camera.performed += ctx =>
            {
                cameraInput = ctx.ReadValue<Vector2>();
            };

            playerControls.PlayerMovement.Jump.performed += ctx =>
            {
                jumpInput = true;
            };

            playerControls.PlayerMovement.Interaction.performed += ctx =>
            {
                interaction_Input = true;   
            };
            playerControls.PlayerActions.WallJump.performed += ctx =>
            {
                wallJumpInput = true;
            };
            playerControls.PlayerActions.RopeSwing.performed += ctx =>
            {
                RopeSwingInput = true;
            };
            playerControls.PlayerActions.RopeSwing.canceled += ctx =>
            {
                RopeSwingInput = false;
            };
            playerControls.PlayerActions.Crouch.performed += ctx =>
            {
                crouchInput = true;
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
        // for calling all the functions , this function itself is called in Update function ;
        HandleMovementInput();
        HandleSprintingInput();
        HandleRopeSwingInput();
        HandleJumpingInput();
        HandleCrouchInput();
    }

    private void HandleMovementInput()
    {
        if (playerLocomotion.rope_climbing)
        {
            if (!playerLocomotion.rope_climbing)
                return;

            horizontalInput = movementInput.x;
            verticalInput = movementInput.y;

            cameraInputX = cameraInput.x;
            cameraInputY = cameraInput.y;

            animatorManager.UpdateAnimatorValues(0, Mathf.Clamp(verticalInput, -1f, 1f), playerLocomotion.is_sprinting);
        }
        else
        {
            horizontalInput = movementInput.x;
            verticalInput = movementInput.y;

            cameraInputX = cameraInput.x;
            cameraInputY = cameraInput.y;

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.is_sprinting);
        }

        if (playerLocomotion.isPlayerCrouching)
        {

            horizontalInput = movementInput.x;
            verticalInput = movementInput.y;

            cameraInputX = cameraInput.x;
            cameraInputY = cameraInput.y;

         //   animatorManager.UpdateAnimatorValues(Mathf.Clamp(horizontalInput, -1f, 1f), Mathf.Clamp(verticalInput, -1f, 1f), playerLocomotion.is_sprinting);
         animatorManager.UpdateAnimatorValues(horizontalInput,verticalInput,playerLocomotion.is_sprinting); 
        }

    }


    private void HandleSprintingInput()
    {
        if(sprintInput && moveAmount > 0.5f)
        {
            playerLocomotion.is_sprinting = true;
        }
        else
        {
            playerLocomotion.is_sprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if (jumpInput)
        {
            jumpInput = false;
            playerLocomotion.HandleJumping();
        }
    }

    public void HandlePullAndPushInputs()
    {
        if(interaction_Input)
        {
            interaction_Input = false;
            playerLocomotion.HandlePushAndPull();
        }
    }

    public void HandleWallJumpInput()
    {
        if(wallJumpInput)
        {
            wallJumpInput = false;
            playerLocomotion.is_wallSliding = false;
            playerLocomotion.HandleWallJump();
        }
    }

    public void HandleRopeSwingInput()
    {
        if(RopeSwingInput)
        {
            playerLocomotion.rope_swinging = true;
        }
        else
        {
            playerLocomotion.rope_swinging = false;
        }
    }

    public void HandleCrouchInput()
    {
        if(crouchInput)
        {
            crouchInput = false;
            playerLocomotion.HandleCrouchMovement();
        }
    }

}
