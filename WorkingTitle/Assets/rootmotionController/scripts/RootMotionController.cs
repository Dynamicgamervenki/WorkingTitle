using System.Collections;
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
    CharacterController _characterController;

    //Jump Values
    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;
    bool jumpPerformed;
    [SerializeField] float JumpForce;



    //Cine machine
    //[SerializeField] CinemachineFreeLook followCam;
    //[SerializeField] CinemachineVirtualCamera aimCam;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController=GetComponent<CharacterController>();
        //followCam.Priority = 15;
    }
    private void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        jumpPerformed=Input.GetKeyDown(KeyCode.Space);
        moveAmount = Mathf.Clamp01(Mathf.Abs(movement.x) + Mathf.Abs(movement.z));
        Locomotion();
        GroundedCheck();
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
        //if(jumpPerformed) { _animator.SetTrigger("Jump"); _characterController.Move(Vector3.up* JumpForce); }
        if (jumpPerformed) 
        {
            StartCoroutine(nameof(Jump)); 
        }
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
    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
       // StopCoroutine(nameof(Jump));
        // update animator if using character
        if (_animator)
        {
            _animator.SetBool("IsGrounded", Grounded);
        }
    }
    [SerializeField] float lerpDuration = 3;
    [SerializeField] float startValue = 0;
    [SerializeField] float endValue = 10;
    float valueToLerp;
    private IEnumerator Jump()
    {
        float timeElapsed = 0;
        _animator.SetTrigger("Jump");
        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            _characterController.Move(Vector3.up * valueToLerp);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        valueToLerp = endValue;
    }

    public void PlayerBalance()
    {
        _animator.SetLayerWeight(2, 1f);
    }
    public void PlayerBalanceComplete()
    {
        _animator.SetLayerWeight(2, 0f);
    }


    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);
    }
}
