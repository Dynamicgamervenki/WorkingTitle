using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Mechanics : MonoBehaviour
{
    [HideInInspector] public Animator anim;
    NewRootMotionController motionController;
    public CharacterController characterController;

    public float rotationSpeed;
    Quaternion targetRotation;
    public bool inControl;
    bool ropeToWallClimbing = false;


    public bool isBalanceWalking = false;

    [Header("Rope Climbing")]
    public float climbSpeed = 0.75f;
    public bool withinRopeRadius = false;
    public bool isRopeClimbing = false;
    public float yOffset;
    public float ropeDetectionRadius = 2.0f;
    public float maxDistance = 2.0f;
    public LayerMask ropeMask;

    public Transform shell;
    public bool waitFor2Sec = false;

    [Header("Cinemachine")]
    public Cinemachine.CinemachineVirtualCamera virtualCamera01;
    public Cinemachine.CinemachineVirtualCamera spiderVirtualCamera;
    public Cinemachine.CinemachineVirtualCamera sideVirtualCamera;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        motionController = FindObjectOfType<NewRootMotionController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (inCutscene)
        {
            gameObject.GetComponent<RootMotionController>().gameObject.SetActive(false);

        }

        Crouch();
        if (isRopeClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 climbMovement = new Vector3(0, verticalInput * climbSpeed * Time.deltaTime, 0);

            if (canClimbEdge)
            {
                motionController.isJumping = false;
            }

            if (verticalInput > 0f)
            {
                anim.SetFloat("moveY", 1.0f);
            }
            else if (verticalInput == 0.0f)
            {
                anim.SetFloat("moveY", 0.0f);
            }
            else if (verticalInput < 0.0f)
            {
                anim.SetFloat("moveY", 2.0f);
            }

        }

    }

    private void FixedUpdate()
    {
        RopeClimbing();
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Balance"))
        {
            Debug.Log("balance started");
            isBalanceWalking = true;
            anim.SetLayerWeight(2, 1.0f);
        }
        else
        {
            isBalanceWalking = false;
            anim.SetLayerWeight(2, 0.0f);
        }
    }

    public bool canClimbEdge = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("climb"))
            canClimbEdge = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("climb"))
            canClimbEdge = false;
    }

    public void OnControl(bool inControl)
    {
        this.inControl = inControl;
        characterController.enabled = inControl;

        if (!inControl)
        {
            anim.SetFloat("Locomotion", 0f);
            targetRotation = transform.rotation;
        }
    }

    bool hasExectuted = false;

    private void RopeClimbing()
    {
        withinRopeRadius = Physics.SphereCast(transform.position + Vector3.up * yOffset, ropeDetectionRadius, transform.forward, out RaycastHit hit, maxDistance, ropeMask);


        if (withinRopeRadius && motionController.isJumping)
        {
            isRopeClimbing = true;
            anim.Play("Rope", 0);
            characterController.enabled = false;
            transform.position = hit.point;
            characterController.enabled = true;
            if (!hasExectuted)
                this.transform.SetParent(hit.transform);
            hit.transform.Rotate(0f, 180f, 0f);
            this.transform.SetParent(null);
            hasExectuted = true;
        }
    }




    public bool isCrouched = false;
    public GameObject crouchObj;
    private void Crouch()
    {
        if (motionController.Grounded && Input.GetKeyDown(KeyCode.C))
        {
            isCrouched = !isCrouched;
        }
        if (isCrouched)
        {
            Debug.Log("Crouched");
            anim.SetBool("isCrouched", true);
            crouchObj.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }
        if (!isCrouched)
        {
            Debug.Log("stand up");
            anim.SetBool("isCrouched", false);
            crouchObj.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
    }


    public GameObject spider;
    public bool inCutscene;
    public GameObject spiderEncounterCutScene;
    public PlayableDirector playableDirector;
    public TimelineAsset timeline;
    public GameObject bossIntroPos;
    public void CameraZoomIn()
    {
        virtualCamera01.Priority = 11;
    }

    public void SpiderZoomIn()
    {
        spiderVirtualCamera.Priority = 12;
        spider.gameObject.SetActive(true);
    }

    public void TriggerSpiderCutscene()
    {
        inCutscene = true;
        virtualCamera01.Priority = 9;
        spiderVirtualCamera.Priority = 9;
        sideVirtualCamera.Priority = 13;
        playableDirector.Play();
    }

    public void UnparentPlayer()
    {
        transform.SetParent(null);
    }





    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * yOffset + transform.forward * maxDistance, ropeDetectionRadius);


    }


}
