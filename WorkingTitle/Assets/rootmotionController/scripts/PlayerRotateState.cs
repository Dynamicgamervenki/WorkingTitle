using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotateState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if(controller==null) 
        //{
        //    controller=animator.GetComponent<PlayerController>();
        //}
        animator.SetLayerWeight(1, 1f);
        PlayerManager.Instance.RootMotionControllerInstance.animationBusy = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float currentTime = stateInfo.normalizedTime * stateInfo.length;
        if (currentTime > .5f && animator.IsInTransition(0))
        {
            animator.rootRotation = PlayerManager.Instance.RootMotionControllerInstance.DesiredRotation;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(1, 0f);
        PlayerManager.Instance.RootMotionControllerInstance.animationBusy = false;
    }
}
