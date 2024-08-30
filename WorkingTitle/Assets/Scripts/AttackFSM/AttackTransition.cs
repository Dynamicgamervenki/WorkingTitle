using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTransition : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("ComboValue", 0);
        animator.applyRootMotion = false;
    }
    [SerializeField] int _comboCount;
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if(PlayerManager.Instance._StarterAssetsInputsInstance.attack)
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetInteger("ComboValue", _comboCount);
        }
                
    }

   
}
