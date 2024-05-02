using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;

    private Transform targetTransform;
    public Transform cameraPivot;
    private Transform cameraTransform;
    public LayerMask collisionLayers;
    private float defaultPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVecrtorPos;


    public float cameraCollisionOffset = 0.2f;
    public float minCollisionOffset = 0.2f;
    public float cameraCollisionRadius = 2.0f;
    public float camerafollowSpeed = 0.2f;
    public float cameraLookSpeed = 2.0f;
    public float cameraPivotSpeed = 2.0f;

    public float lookAngle;                                         // up and down
    public float pivotAngle;                                        // left and right
    public float minPivotAngle = -35;
    public float maxPivotAngle = 35;

    private void Awake()
    {
        inputManager =  FindObjectOfType<InputManager>();
        targetTransform = FindObjectOfType<PlayerManagers>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    private void FollowTarget()
    {
        Vector3 taregtPos = Vector3.SmoothDamp(transform.position,targetTransform.position,ref cameraFollowVelocity,camerafollowSpeed);

        transform.position = taregtPos;
    }

    private void RotateCamera()
    {
        lookAngle = lookAngle + (inputManager.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputManager.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle,minPivotAngle,maxPivotAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollisions()
    {
        float targetposition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if(Physics.SphereCast(cameraPivot.transform.position,cameraCollisionRadius,direction,out hit,Mathf.Abs(targetposition),collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position,hit.point);
            targetposition =-  (distance - cameraCollisionOffset);
        }

        if(Mathf.Abs(targetposition) < minCollisionOffset)
        {
            targetposition =- targetposition - minCollisionOffset;
        }

        cameraVecrtorPos.z = Mathf.Lerp(cameraTransform.localPosition.z,targetposition,0.2f);
        cameraTransform.localPosition = cameraVecrtorPos;
    }

    public void HandleAllCameraMovemnts()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }
}
