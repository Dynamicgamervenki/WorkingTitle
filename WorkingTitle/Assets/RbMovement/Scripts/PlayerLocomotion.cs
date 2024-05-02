using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private Vector3 moveDirection;
    private Transform camera;
    private InputManager inputManager;
    private Rigidbody rb;

    [Header("Movemnt")]
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float rotationSpeed = 10.0f;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        camera = Camera.main.transform;
    }

    private void HandleMovement()
    {
        moveDirection = camera.forward * inputManager.verticalInput;
        moveDirection = moveDirection + camera.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = camera.forward * inputManager.verticalInput;
        targetDirection = targetDirection + camera.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation,targetRotation,rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    public void HandleAllMovements()    //reagrding rigidbody ,will be called in fixed update
    {
        HandleMovement();
        HandleRotation();   
    }
}
