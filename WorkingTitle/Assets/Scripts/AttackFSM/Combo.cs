using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    [SerializeField]float _keyFrameMin,_keyFrameMax;
    [SerializeField] bool _canReciveInput;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.applyRootMotion = true;
        PlayerManager.Instance._ThirdPersonControllerInstance._canMove = false;
        Debug.LogError("t");
        _canReciveInput=true;
    }
    [SerializeField] int _comboCount;
    //MonoBehaviour mono = new MonoBehaviour();
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerManager.Instance._StarterAssetsInputsInstance.inputActions.Player.Fire1.WasPressedThisFrame() && stateInfo.normalizedTime> _keyFrameMin && stateInfo.normalizedTime < _keyFrameMax && _canReciveInput)
        {
            
            _canReciveInput=false;
            animator.SetInteger("ComboValue", _comboCount);
            //mono.StartCoroutine(Motion(stateInfo.length,animator));
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if(stateInfo.length>1)
       // animator.applyRootMotion = false;
        _canReciveInput = true;
    }
    IEnumerator Motion(float time,Animator anim)
    {
        
        yield return new WaitForSeconds(time);
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
