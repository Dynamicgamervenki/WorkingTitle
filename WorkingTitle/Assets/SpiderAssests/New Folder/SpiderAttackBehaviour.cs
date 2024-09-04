using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAttackBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    EnemyBehaviour behaviour;
    [SerializeField] float minTime=0.5f, maxTime=1f;
    [SerializeField] int attackCounter;
    [SerializeField] float attackDistance, detectStart, detectEnd;
    [SerializeField] bool detect;
    [SerializeField] float healthToDetect=15f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(behaviour==null&& animator.TryGetComponent(out EnemyBehaviour enemy)) 
        {
            behaviour = enemy;
        }
        behaviour.TimeToAttack = Random.Range(minTime, maxTime);
        detect = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(behaviour.ReturnDistance() < attackDistance)
        {
            if (Time.time - behaviour.TimerCounter <= behaviour.TimeToAttack)
            {
                behaviour.TimerCounter = Time.time;
                animator.SetInteger("Attack", attackCounter);
            }
        }
        else
        {
            animator.SetInteger("Attack", 0);
        }
        if(stateInfo.normalizedTime>=detectStart && stateInfo.normalizedTime<=detectEnd && detect)
        {
            PlayerManager.Instance.NewRootMotionControllerInstance.playerAnimator().SetTrigger("PlayerHit");
            PlayerManager.Instance.PlayerHealth.setHealth(healthToDetect);
            detect = false;
        }

    }
}
