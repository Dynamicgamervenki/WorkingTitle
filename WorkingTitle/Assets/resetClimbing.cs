using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetClimbing : StateMachineBehaviour
{

    Mechanics mechanics;

    private void Awake()
    {
        mechanics = FindObjectOfType<Mechanics>();
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mechanics.shell.transform.GetChild(0).gameObject.SetActive(false);
       
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //  mechanics.shell.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        mechanics.characterController.center = new Vector3(0, 2.86999989f, -0.1f);
        mechanics.isRopeClimbing = false;
        mechanics.canClimbEdge = false; 
        mechanics.anim.SetBool("isRopeClimbing",false);
        
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
