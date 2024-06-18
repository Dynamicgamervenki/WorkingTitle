using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderWalkStateBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    EnemyBehaviour behaviour;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(behaviour==null&&animator.TryGetComponent(out EnemyBehaviour enemy))
        {
            behaviour = enemy;
        }
        behaviour.agent.isStopped = false;

    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (temp != null)
        //{
        //    if ()
        //    {
        //        //Debug.LogError(Vector3.Distance(animator.transform.position, temp.position));
        //        agent.SetDestination(temp.position+Vector3.one);
        //    }
        //    else
        //    {
        //        agent.SetDestination(animator.transform.position);
        //        animator.SetTrigger("Attack");
        //    }
        //}
        Debug.LogError(behaviour.ReturnDistance() > behaviour.JumpDistanceMin);
        Debug.LogError(behaviour.ReturnDistance() );
        if (behaviour.ReturnDistance() > behaviour.JumpDistanceMin && behaviour.ReturnDistance() < behaviour.JumpDistanceMax)
        {
            if (Time.time - behaviour.JumpTimerCounter >= behaviour.TimeToJumpAttack)
            {
                behaviour.JumpTimerCounter = Time.time;
                animator.SetTrigger("Jump");
            }
        }
        behaviour.agent.SetDestination(behaviour.playerREF.position);
        behaviour.EnemyLookAtPlayer();
        animator.SetBool("Walk", behaviour.ReturnDistance() > behaviour.attackDistance);

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
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
