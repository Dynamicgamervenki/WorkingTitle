using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.enabled = false;
    }


}
