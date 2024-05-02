using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerComboSystem : MonoBehaviour
{
    public List<AttackData> comboDataHeavy;
    public List<AttackData> comboDataMid;
    public List<AttackData> Temp;
    public int comboCount;
    
    Animator animator;

    public bool playerRotate;
    float lastClickedTime;
    float lastComboEnd;
    //public float comboToEnd;
    //public float clickToEnd;
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        //PlayerManger.instance.controllerInstance.playerInputActions.Player.Attack.started += Combo;
        //PlayerManger.instance.controllerInstance.PlayerActions += ExitAttack;
        PlayerManager.Instance._StarterAssetsInputsInstance.inputActions.Player.Fire1.started += Combo;
        Temp = comboDataMid;
        _meleeId = Animator.StringToHash("Melee");
    }

    public void Combo(InputAction.CallbackContext callback)
    {
        CancelInvoke(nameof(ExitCombo));
        if(comboCount> Temp.Count-1) {
            comboCount = 0;
        }
        //Debug.LogError(lastComboEnd);
        if (Time.time - lastComboEnd > Temp[comboCount].InputReciveTime && comboCount< Temp.Count) 
        {
            //Debug.LogError("combo");
            lastComboEnd = Time.time;
            if (Time.time-lastClickedTime>=0.5f)
            {
                //Debug.LogError("click");
                StartCoroutine(PlayerComboAnimation(Temp[comboCount]));
                comboCount++;
                lastClickedTime = Time.time;
                
            }
        }
        else
        {
            return;
        }
    }
    IEnumerator PlayerComboAnimation(AttackData data)
    {
        PlayerManager.Instance._ThirdPersonControllerInstance._canMove = false;
        animator.applyRootMotion = true;
        //playerRotate = true;
        //animator.Play(data.AnimationName);
        Debug.Log("no");
        animator.CrossFade(data.AnimationName, 0.2f);
        //animator.Play(data.AnimationName);
        yield return new WaitForSeconds(data.endTime);
        animator.applyRootMotion = false;
        Debug.Log("yes");
        playerRotate = false;
        PlayerManager.Instance._ThirdPersonControllerInstance._canMove = true;
    }

    void ExitCombo()
    {
        comboCount = 0;
        lastComboEnd = Time.time;
      
    }
    public void ExitAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).length > 0.9 && animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack"))
        {
            //Debug.LogError("end"+lastComboEnd);
            Invoke(nameof(ExitCombo), 1.5f);
        }
    }
    public bool flag;
    int _meleeId;
    private void EquipAndUnequip(InputAction.CallbackContext context)
    {
        flag=!flag;
        animator.SetBool(_meleeId, flag);
    }
}
