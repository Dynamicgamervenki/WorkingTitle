using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTransition : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    animator.SetInteger("ComboValue", 0);
    //    animator.applyRootMotion = false;
    //    PlayerManager.Instance._ThirdPersonControllerInstance._canMove = true;
    //    PlayerManager.Instance._ThirdPersonControllerInstance._swordCollider.enabled = false;
    //}
    [SerializeField] int _comboCount;
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if(PlayerManager.Instance._StarterAssetsInputsInstance.attack)
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.LogError("attack");
            animator.SetInteger("ComboValue", _comboCount);
        }
                
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("ComboValue", 0);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
