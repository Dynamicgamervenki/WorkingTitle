using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RopeClimb : MonoBehaviour
{
    bool ropeClimb;
    Animator animator;
    int ropeClimbId = Animator.StringToHash("RopeClimb");
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }
    public void RopeClimbFunction(InputAction.CallbackContext callbackContext)
    {
        ropeClimb = !ropeClimb;

        if (Physics.SphereCast(transform.position, 0.5f, Vector3.forward, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.transform.gameObject.tag == "Rope")
            {
                animator.SetBool("ropeClimbId", ropeClimb);
            }
        }
    }
}
