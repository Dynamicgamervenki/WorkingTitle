using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderIdleBehaviour : StateMachineBehaviour
{
    EnemyBehaviour behaviour;
   
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(behaviour == null && animator.TryGetComponent(out EnemyBehaviour enemy )) 
        { 
            behaviour = enemy;
        }
        behaviour.agent.isStopped = true;
        behaviour.EnemyLookAtPlayer();
       
    }
   
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        behaviour.EnemyLookAtPlayer();
        Debug.LogError(behaviour.ReturnDistance() > behaviour.JumpDistanceMin);
        Debug.LogError(behaviour.ReturnDistance());
        if (behaviour.ReturnDistance() > behaviour.JumpDistanceMin && behaviour.ReturnDistance() < behaviour.JumpDistanceMax)
        {
            if(Time.time - behaviour.JumpTimerCounter >= behaviour.TimeToJumpAttack)
            {

                behaviour.JumpTimerCounter = Time.time;
                animator.SetTrigger("Jump");
            }
        }
        animator.SetBool("Walk", behaviour.ReturnDistance() > behaviour.attackDistance);
        if (Time.time -behaviour.TimerCounter >= behaviour.TimeToAttack)
        {
            behaviour.TimerCounter = Time.time;
            animator.SetInteger("Attack",1);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
