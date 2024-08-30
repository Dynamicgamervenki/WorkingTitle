using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    [SerializeField]float _keyFrameMin,_keyFrameMax;
    [SerializeField] bool _canReciveInput;
    [SerializeField]float swordDetectEnableTime,swordDetectDisableTime;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("t");
        _canReciveInput=true;
    }
    [SerializeField] int _comboCount;
    //MonoBehaviour mono = new MonoBehaviour();
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) /*&& stateInfo.normalizedTime> _keyFrameMin && stateInfo.normalizedTime < _keyFrameMax && _canReciveInput)*/
        {
            _canReciveInput=false;
            animator.SetInteger("ComboValue", _comboCount);
            //mono.StartCoroutine(Motion(stateInfo.length,animator));
        }
       
        
            if (stateInfo.normalizedTime >= swordDetectEnableTime && stateInfo.normalizedTime <= swordDetectDisableTime)
            {
                
                if (PlayerManager.Instance.AttackDetect.ReturnCollider().transform.TryGetComponent(out Animator component) &&_canReciveInput)
                {
                    if(PlayerManager.Instance.AttackDetect.ReturnCollider().transform.TryGetComponent(out EnemyHealthScript refhealth))
                    {
                        refhealth.GetComponent<EnemyHealthScript>().setHealth(15);
                    }
                   Debug.Log("Enemy Hit");
                   component.SetTrigger("Hit"); _canReciveInput = false; 
                }
                //Debug.Log(PlayerManager.Instance.AttackDetect.ReturnCollider().transform);
            }
        

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if(stateInfo.length>1)
       // animator.applyRootMotion = false;
       // _canReciveInput = true;
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
