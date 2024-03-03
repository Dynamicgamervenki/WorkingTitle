using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private PlayerInput input;
    private Vector2 currentMovement;
    private bool movment_Pressed;
    private bool run_Pressed;

    int isWalkingHash;
    int isRunningHash;

    private void Awake()
    {
        input = new PlayerInput();

        input.Player.Move.performed += ctx =>
        {
            currentMovement = ctx.ReadValue<Vector2>();
            movment_Pressed = currentMovement.x != 0 || currentMovement.y != 0;
        };

        input.Player.Run.performed += ctx =>
        {
           run_Pressed =  ctx.ReadValueAsButton();
        };

        input.Player.Move.canceled += ctx =>
        {
            currentMovement = Vector2.zero;
            movment_Pressed = false;
        };

        input.Player.Run.canceled += ctx =>
        {
            run_Pressed = false;
        };
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }
    private void Update()
    {
        Movement();
        Rotation();
    }

    private void Movement()
    {
        bool isWalking = anim.GetBool(isWalkingHash);
        bool isRunning = anim.GetBool(isRunningHash);   

        if(movment_Pressed && !isWalking)
        {
            anim.SetBool(isWalkingHash, true);
        }

        if(!movment_Pressed && isWalking)
        {
            anim.SetBool(isWalkingHash, false);
        }

        if(movment_Pressed && run_Pressed && !isRunning)
        {
            anim.SetBool(isRunningHash,true);
        }

        if (!movment_Pressed && !run_Pressed && isRunning)
        {
            anim.SetBool(isRunningHash, false);
        }


    }

    private void Rotation()
    {
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = new Vector3(currentMovement.x,0,currentMovement.y);
        Vector3 positionToLookAt = currentPosition + newPosition;
        transform.LookAt(positionToLookAt);
    }

    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable(); 
    }
}
