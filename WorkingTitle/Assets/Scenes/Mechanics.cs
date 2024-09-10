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

    public Transform[] shell;
    public bool waitFor2Sec = false;

    [Header("Cinemachine")]
    public Cinemachine.CinemachineVirtualCamera virtualCamera01;
    public Cinemachine.CinemachineVirtualCamera spiderVirtualCamera;
    public Cinemachine.CinemachineVirtualCamera sideVirtualCamera;

    public GameObject Spider;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        motionController = FindObjectOfType<NewRootMotionController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        WallClimbing();
        Crouch();

        if (isRopeClimbing || isWallClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");

            if (canClimbEdge)
            {
                motionController.isJumping = false;
            }

            if(isWallClimbing && motionController.Grounded && verticalInput < 0)
            {
                isWallClimbing = false;
                anim.Play("In Air");
            }

            float moveY = verticalInput > 0 ? 1.0f : verticalInput < 0 ? 2.0f : 0.0f;
            anim.SetFloat("moveY", moveY, 0.1f, Time.deltaTime); 

        }

        if (Input.GetKeyDown(KeyCode.E) && !weaponEquipped && needleWithinRadius)
        {
            Debug.Log("Weapon picked");
            HandIkTarget.position = Needle.transform.position;
            anim.SetTrigger("pickup");
            weaponEquipped = true;
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
            anim.SetBool("isCrouched", true);
            crouchObj.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }
        if (!isCrouched)
        {
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
    public bool damned = false;
    public void CameraZoomIn()
    {
        spider.SetActive(true);
        virtualCamera01.Priority = 11;
        damned = true;
        anim.Play("Walk");
    }

    public void SpiderZoomIn()
    {
        spider.gameObject.SetActive(true);
        spiderVirtualCamera.Priority = 12;
        
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

    public void Damn()
    {
        damned = false;
    }

    public bool needleWithinRadius = false;
    public Transform HandIkTarget;
    public Transform HandBone;
    public GameObject Needle;

    public bool weaponEquipped = false;
    public void NeedleIsWithinRadius()
    {
        needleWithinRadius = true;
    }

    public void NeedleNotWithInRadius()
    {
        needleWithinRadius = false;
    }



    private void GrabNeedle()
    {
        Needle.transform.SetParent(HandBone);
    }

    public void NeeleSetActiveFalse()
    {
        StartCoroutine(SetActiveFalses());
    }

    IEnumerator SetActiveFalses()
    {
        yield return new WaitForSeconds(1.0f);
        Needle.SetActive(false);
    }

    public bool  isWallClimbing = false;
    public LayerMask wallMask;
    public ParkourAction climbUp;
    public GameObject WallClimbLimit;
    public void WallClimbing()
    {
        if(Physics.Raycast(transform.position + Vector3.up * 0.5f,transform.forward,out RaycastHit hit,0.15f,wallMask))
        {
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, transform.forward * 0.15f);
            if (Input.GetKey(KeyCode.Space))
            {
                WallClimbLimit.SetActive(true);
                isWallClimbing = true;
                anim.Play("WallClimbStart");
            }
        }

        if(Input.GetKey(KeyCode.E) && isWallClimbing)
        {
            ParkourController pc = FindAnyObjectByType<ParkourController>();
            StartCoroutine(pc.DoClimbAction(climbUp));
            WallClimbLimit.gameObject.SetActive(false);
        }

    }



    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * yOffset + transform.forward * maxDistance, ropeDetectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.5f);

    }


}
