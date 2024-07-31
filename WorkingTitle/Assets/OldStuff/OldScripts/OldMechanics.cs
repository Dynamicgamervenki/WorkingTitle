using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class OldMechanics : MonoBehaviour
{
    private Animator anim;
    public PlayerInputs input;
    private Vector2 currentMovement;
    private bool movment_Pressed;
    private bool run_Pressed;
    public bool rope_Climb;
    public bool rope_ClimbInProgress;
    [SerializeField] private float rotationSpeed = 5.0f;

    int isWalkingHash;
    int isRunningHash;

    //Actions 
    public UnityAction playerActions;

    private void Awake()
    {
        input = new PlayerInputs();

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
        input.Player.Interact.performed += ctx => 
        { 
            rope_Climb = !rope_Climb;
            anim.SetBool("CanRopeClimb", rope_Climb);
            if(rope_Climb)
            {
                playerActions -= Movement;
                playerActions -= Rotation;
                playerActions += RopeClimb;
            }
            if(!rope_Climb)
            {
                playerActions += Movement;
                playerActions += Rotation;
                playerActions -= RopeClimb;
            }
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
        //Movement();
        //Rotation();
        playerActions?.Invoke();
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
        Vector3 newPosition = new Vector3(currentMovement.x, 0, currentMovement.y);
        Vector3 positionToLookAt = currentPosition + newPosition;

        Vector3 direction = positionToLookAt - transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void RopeClimb()
    {
        anim.SetInteger("RopeClimb", (int)(input.Player.Move.ReadValue<Vector2>().y));
    }
    private void OnEnable()
    {
        input.Player.Enable();
        playerActions += Movement;
        playerActions += Rotation;
    }

    private void OnDisable()
    {
        input.Player.Disable();
        playerActions -= Movement;
        playerActions -= Rotation;
    }
}
