using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderJumpBehaviour : AttackTransition
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    EnemyBehaviour behaviour;
    Transform destination;
    [SerializeField]
    float speed;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(behaviour == null && animator.TryGetComponent(out EnemyBehaviour component)) { behaviour = component; }

        behaviour.agent.SetDestination(behaviour.playerREF.position);
        behaviour.agent.speed = speed;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        behaviour.agent.speed = 1;
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
