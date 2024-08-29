using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRootMotionController : MonoBehaviour
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
    ParkourController parkourController;

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
    public bool  isJumping;
    [SerializeField] float jumpHeight, gravity;
    Vector3 velocity;

    [SerializeField] float stepDown,airControl,jumpDamp=.5f;

    Vector3 rootMotion;

    Mechanics mechanics;


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        parkourController = FindObjectOfType<ParkourController>();
        Cursor.lockState = CursorLockMode.Locked;
        
        mechanics = GetComponent<Mechanics>();
    }
    bool canDoubleJump;
    private void Update()
    {

        movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        //if(!mechanics.inControl)
        //    return;
        GroundedCheck();
        if (mechanics.isRopeClimbing)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        moveAmount = Mathf.Clamp01(Mathf.Abs(movement.x) + Mathf.Abs(movement.z));
        Locomotion();
        

    }
    void Locomotion()
    {
        if(mechanics.isRopeClimbing)
            return;

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
                //if (_animator.GetFloat("Locomotion") > 1f)
                //{
                //    _animator.CrossFade("Running turn 180", .15f);
                //}
                //else
                //{
                //    _animator.CrossFade("Walk Turn 180", .05f);
                //}
                // anim.SetTrigger("Turn180");
                Vector3 anim_rotation = _animator.rootRotation.eulerAngles;
                DesiredRotation = Quaternion.Euler(new Vector3(anim_rotation.x, anim_rotation.y + 180f, anim_rotation.z));
            }
            target = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, degreeDelta * Time.deltaTime);
        }
    }
    //private void OnAnimatorMove()
    //{
    //    rootMotion += _animator.deltaPosition;
    //}
    private void FixedUpdate()
    {
        if (mechanics.isRopeClimbing)
            return;

        if (isJumping)
        {
            velocity.y -= gravity * Time.fixedDeltaTime;
            Vector3 displacement = velocity * Time.fixedDeltaTime;
            displacement += CalculateAirContoll();
            _characterController.Move(displacement);
            //isJumping = !_characterController.isGrounded;
            isJumping = !_characterController.isGrounded;
            rootMotion = Vector3.zero;
            canDoubleJump = true;
        }
        else
        {
            //if (mechanics.isRopeClimbing || mechanics.canClimbEdge)
            //    return;
            if (mechanics.withinRopeRadius || mechanics.isRopeClimbing)
                return;

            _characterController.Move(rootMotion + Vector3.down * stepDown);
            rootMotion = Vector3.zero;
            if (!_characterController.isGrounded)
            {
                isJumping = true;
                velocity = _animator.velocity * jumpDamp;
                velocity.y = 0;
            }
        }

    }
    int jumpCount;
    void Jump()
    {
        if (mechanics.isRopeClimbing)
            return;
        if (mechanics.isCrouched)
            return;
        if (mechanics.canClimbEdge)
            return;

        if (_characterController.isGrounded)
        {
            jumpCount++;
            _animator.SetTrigger("Jump");
            isJumping = true;
            velocity = _animator.velocity * jumpDamp;
            velocity.y = Mathf.Sqrt(2 * gravity * jumpHeight);
        }
        //else
        //{
        //    if (jumpCount > 1)
        //    {
        //        _animator.SetTrigger("DoubleJump");
        //        isJumping = true;
        //        velocity = _animator.velocity * jumpDamp;
        //        velocity.y = Mathf.Sqrt(2 * gravity * jumpHeight);
        //        jumpCount = 0;
        //    }
        //}
    }
    Vector3 CalculateAirContoll()
    {
        return ((Vector3.forward * movement.z) + (Vector3.right * movement.x)) * (airControl/100);
        //return ((Vector3.forward * movement.z)) * (airControl/100);
        //return ((transform.InverseTransformPoint(transform.forward) * movement.z) + (transform.InverseTransformPoint(transform.right) * movement.x)) * (airControl / 100);
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
            _animator.SetBool("FreeFall", !Grounded);
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


    private void OnDrawGizmosSelected()
    {
        //Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        //Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        //if (Grounded) Gizmos.color = transparentGreen;
        //else Gizmos.color = transparentRed;

        //// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        //Gizmos.DrawSphere(
        //    new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
        //    GroundedRadius);
    }
}
