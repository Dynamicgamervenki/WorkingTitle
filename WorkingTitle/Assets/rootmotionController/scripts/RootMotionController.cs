using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class RootMotionController : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 movement;
    [SerializeField] float moveAmount;
    Quaternion target;
    [SerializeField] float degreeDelta = 500;
    Vector3 cam;
    float turnAngle;
    [HideInInspector] public Quaternion DesiredRotation;
    //Animation Values
    [SerializeField, Range(0f, 1f)] private float damValue;
    float _runValue;
    [HideInInspector] public bool animationBusy;
    Animator _animator;


    //Cine machine
    //[SerializeField] CinemachineFreeLook followCam;
    //[SerializeField] CinemachineVirtualCamera aimCam;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        //followCam.Priority = 15;
    }
    private void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        moveAmount = Mathf.Clamp01(Mathf.Abs(movement.x) + Mathf.Abs(movement.z));
        Locomotion();
        //_animator.SetFloat("moveX", movement.x, damValue, Time.deltaTime);
        //_animator.SetFloat("moveY", movement.z, damValue, Time.deltaTime);

    }

    void AimMovement()
    {
       
    }
    void Locomotion()
    {
        _runValue = (Input.GetKey(KeyCode.LeftShift)) ? 5f : 1f;
        _animator.SetFloat("Locomotion", moveAmount * _runValue, damValue, Time.deltaTime);

        movement = (Quaternion.LookRotation(new Vector3(cam.x, 0f, cam.z)) * movement);
        if (animationBusy) { return; }
        if (moveAmount > 0f)
        {
            cam = Camera.main.transform.forward;
            turnAngle = Mathf.Abs(Vector3.SignedAngle(transform.forward, movement, Vector3.up));
            if (turnAngle >= 175f)
            {
                if (_animator.GetFloat("Locomotion") > 1f)
                {
                    _animator.CrossFade("Running turn 180", .15f);
                }
                else
                {
                    _animator.CrossFade("Walk Turn 180", .05f);
                }
                // anim.SetTrigger("Turn180");
                Vector3 anim_rotation = _animator.rootRotation.eulerAngles;
                DesiredRotation = Quaternion.Euler(new Vector3(anim_rotation.x, anim_rotation.y + 180f, anim_rotation.z));
            }
            target = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, degreeDelta * Time.deltaTime);
        }
    }

    public void PlayerBalance()
    {
        _animator.SetLayerWeight(2, 1f);
    }
    public void PlayerBalanceComplete()
    {
        _animator.SetLayerWeight(2, 0f);
    }
}
